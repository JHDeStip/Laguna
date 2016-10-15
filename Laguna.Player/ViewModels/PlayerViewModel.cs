using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Stannieman.AudioPlayer;
using Stannieman.CacheTemp;
using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Domain;
using JhDeStip.Laguna.TrackProvider;
using JhDeStip.Laguna.Player.Messages;
using JhDeStip.Laguna.Player.Config;
using System.Windows;
using JhDeStip.Laguna.Player.Resources;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        #region Instance variables and constants

        // Constants
        private const double RetryInterval = 5000;
        private const double RefreshDownloadQueueInterval = 10000;
        private static readonly TrackPosition DefaultTrackPosition = new TrackPosition();

        private TrackDownloadConfig _downloadConfig;

        // Services
        private IAudioPlayer _audioPlayer;
        private IPlaylistService _playlistService;
        private ITrackDownloader _trackDownloader;
        private ICacheTempManager _cacheTempManager;

        private SynchronizationContext _context;
        private PlayableItemInfo _playingItem;
        private System.Timers.Timer _retryTimer;
        private System.Timers.Timer _refreshDownloadQueueTimer;
        private string _waitingForDownloadTrackId = null;
        private bool _isRefreshingDownloadQueue;
        private bool _listenForButtons;

        #endregion

        #region Properties for view

        private TrackPosition _trackPosition;
        public TrackPosition TrackPosition
        {
            get
            {
                return _trackPosition;
            }
            set
            {
                if (!value.Equals(_trackPosition))
                {
                    _trackPosition = value;
                    RaisePropertyChanged(nameof(TrackPosition));
                }
            }
        }
        private TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get
            {
                return _totalTime;
            }
            set
            {
                if (!value.Equals(_totalTime))
                {
                    _totalTime = value;
                    RaisePropertyChanged(nameof(TotalTime));
                }
            }
        }
        private bool _progressBarDownloadMode;
        public bool ProgressBarDownloadMode
        {
            get
            {
                return _progressBarDownloadMode;
            }
            set
            {
                if (value != _progressBarDownloadMode)
                {
                    _progressBarDownloadMode = value;
                    RaisePropertyChanged(nameof(ProgressBarDownloadMode));
                }
            }
        }
        private string _thumbnailImageUrl;
        public string ThumbnailImageUrl
        {
            get
            {
                return _thumbnailImageUrl;
            }
            set
            {
                if (value != _thumbnailImageUrl)
                {
                    _thumbnailImageUrl = value;
                    RaisePropertyChanged(nameof(ThumbnailImageUrl));
                }
            }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }
        private bool _nextTrackButtonEnabled;
        public bool NextTrackButtonEnabled
        {
            get
            {
                return _nextTrackButtonEnabled;
            }
            set
            {
                if (value != _nextTrackButtonEnabled)
                {
                    _nextTrackButtonEnabled = value;
                    RaisePropertyChanged(nameof(NextTrackButtonEnabled));
                }
            }
        }
        private bool _playPauseButtonEnabled;
        public bool PlayPauseButtonEnabled
        {
            get
            {
                return _playPauseButtonEnabled;
            }
            set
            {
                if (value != _playPauseButtonEnabled)
                {
                    _playPauseButtonEnabled = value;
                    RaisePropertyChanged(nameof(PlayPauseButtonEnabled));
                }
            }
        }
        private Visibility _playButtonVisibility;
        public Visibility PlayButtonVisibility
        {
            get
            {
                return _playButtonVisibility;
            }
            set
            {
                if (value != _playButtonVisibility)
                {
                    _playButtonVisibility = value;
                    RaisePropertyChanged(nameof(PlayButtonVisibility));
                }
            }
        }
        private Visibility _pauseButtonVisibility;
        public Visibility PauseButtonVisibility
        {
            get
            {
                return _pauseButtonVisibility;
            }
            set
            {
                if (value != _pauseButtonVisibility)
                {
                    _pauseButtonVisibility = value;
                    RaisePropertyChanged(nameof(PauseButtonVisibility));
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand PlayCommand { get; private set; }
        public RelayCommand PauseCommand { get; private set; }
        public RelayCommand NextTrackCommand { get; private set; }

        #endregion

        /// <summary>
        /// Constructor accespting parameters for all required services.
        /// </summary>
        /// <param name="audioPlayer">IAudioPlayer instance.</param>
        /// <param name="playlistService">IPlaylistService instance.</param>
        /// <param name="trackDownloader">ITrackDownloader instance.</param>
        /// <param name="cacheManager">ICacheManager instance.</param>
        public PlayerViewModel(TrackDownloadConfig downloadConfig, IAudioPlayer audioPlayer, IPlaylistService playlistService, ITrackDownloader trackDownloader, ICacheTempManager cacheTempManager)
        {
            _downloadConfig = downloadConfig;
            _audioPlayer = audioPlayer;
            _audioPlayer.FinishedPlaying += OnTrackFinished;
            _audioPlayer.PositionChanged += OnPositionChanged;
            _playlistService = playlistService;
            _trackDownloader = trackDownloader;
            _trackDownloader.TrackDownloadCompleted += OnTrackDownloadCompleted;
            _trackDownloader.TrackDownloadFailed += OnTrackDownloadFailed;
            _cacheTempManager = cacheTempManager;

            _context = SynchronizationContext.Current;
            _retryTimer = new System.Timers.Timer(RetryInterval);
            _retryTimer.Elapsed += OnRetryTimerElapsed;
            _refreshDownloadQueueTimer = new System.Timers.Timer(RefreshDownloadQueueInterval);
            _refreshDownloadQueueTimer.Elapsed += OnRefreshDownloadQueueTimerElapsed;
            _isRefreshingDownloadQueue = false;

            PlayButtonVisibility = Visibility.Collapsed;
            PauseButtonVisibility = Visibility.Visible;

            Messenger.Default.Register<RefreshDownloadQueueMessage>(this, async (refreshDownloadQueueMessage) => await RefreshDownloadQueue());
            Messenger.Default.Register<ShutDownMessage>(this, OnShutDownMessage);

            CreateCommands();
        }

        /// <summary>
        /// Creates the commands.
        /// </summary>
        private void CreateCommands()
        {
            LoadedCommand = new RelayCommand(OnLoaded);
            PlayCommand = new RelayCommand(OnPlay);
            PauseCommand = new RelayCommand(OnPause);
            NextTrackCommand = new RelayCommand(OnNext);
        }

        /// <summary>
        /// Initiates the precodure to play the next track.
        /// This runs when the retry timer elapses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">ElapsedEventArgs.</param>
        private async void OnRetryTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // We don't want this to run again.
            ((System.Timers.Timer)sender).Stop();

            await GetAndPlayNextTrack();
        }

        /// <summary>
        /// Refreshes the download queue.
        /// This runs when the refresh download queue timer elapses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">ElapsedEventArgs.</param>
        private void OnRefreshDownloadQueueTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Run in the UI thread.
            _context.Post(new SendOrPostCallback(async (state) =>
            {
                // We don't want this again until we are ready
                ((System.Timers.Timer)sender).Stop();

                await RefreshDownloadQueue();

                // The job is done, so schedule it again
                ((System.Timers.Timer)sender).Start();
            }), null);
        }

        /// <summary>
        /// Gets the info for the next track and calls the PlayNextTrack method to play it.
        /// </summary>
        /// <returns>Task instance.</returns>
        private async Task GetAndPlayNextTrack()
        {
            _listenForButtons = false;
            try
            {
                _playingItem = await _playlistService.GetNextAndSetAsNowPlayingAsync();

                if (_playingItem == null)
                {
                    Title = UIStrings.NoTracksToPlay;

                    // We try again after the set interval
                    _retryTimer.Start();
                }
                else
                    await PlayNextTrack();
            }
            catch (Exception e)
            {
                Title = UIStrings.NoTracksToPlay;

                _retryTimer.Start();
            }
        }

        /// <summary>
        /// Starts playback of the track who's info is set.
        /// </summary>
        /// <returns>Task instance.</returns>
        private async Task PlayNextTrack()
        {
            // Make sure the player is stopped
            await _audioPlayer.StopAsync();

            // Show track info on the player
            ThumbnailImageUrl = _playingItem.ThumbnailUrl;
            Title = _playingItem.Title;
            TotalTime = _playingItem.Duration;

            string fullFileName = _cacheTempManager.GetFullFileNameWithoutExtension(_playingItem.ItemId);

            // If the file is found (GetFullFileName didn't return null) start playing the file, otherwise start downloading it
            if (fullFileName != null)
            {
                await _audioPlayer.SetFileAsync(fullFileName, _playingItem.ItemId);
                await _audioPlayer.PlayAsync();

                ProgressBarDownloadMode = false;
                PlayPauseButtonEnabled = true;
                PlayButtonVisibility = Visibility.Collapsed;
                PauseButtonVisibility = Visibility.Visible;

                if (!_isRefreshingDownloadQueue)
                {
                    _refreshDownloadQueueTimer.Stop();
                    // We started playing, so start downloading next N tracks
                    await RefreshDownloadQueue();
                    _refreshDownloadQueueTimer.Start();
                }
            }
            else
            {
                PlayPauseButtonEnabled = false;
                PlayButtonVisibility = Visibility.Visible;
                PauseButtonVisibility = Visibility.Collapsed;
                _waitingForDownloadTrackId = _playingItem.ItemId;
                Title = $"{Title} {UIStrings.PlayerTitle_Downloading}";
                ProgressBarDownloadMode = true;
                await _trackDownloader.SetHighestPriorityTrack(_playingItem.ItemId);
            }

            NextTrackButtonEnabled = true;

            Messenger.Default.Send(new RefreshListMessage());

            _listenForButtons = true;
        }

        private async void OnLoaded()
        {
            Title = UIStrings.Loading;
            await GetAndPlayNextTrack();
        }

        /// <summary>
        /// Runs after the download of a track completed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">TrackDownloadCompleteEventArgs.</param>
        private void OnTrackDownloadCompleted(object sender, TrackDownloadCompletedEventArgs e)
        {
            // Run in the UI thread.
            _context.Post(new SendOrPostCallback(async (state) =>
            {
                _listenForButtons = false;
                // If this was the track we were waiting for, start playing it
                if (e.TrackId == _waitingForDownloadTrackId)
                {
                    _waitingForDownloadTrackId = null;
                    await PlayNextTrack();
                }
                _listenForButtons = true;
            }), null);
        }

        /// <summary>
        /// Runs when the download of a track failed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">TrackDownloadFailedEventArgs.</param>
        private void OnTrackDownloadFailed(object sender, TrackDownloadFailedEventArgs e)
        {
            // Run in the UI thread.
            _context.Post(new SendOrPostCallback(async (state) =>
            {
                _listenForButtons = false;
                // If this was the track we were waiting for, skip to the next track
                if (e.TrackId == _waitingForDownloadTrackId)
                {
                    _waitingForDownloadTrackId = null;
                    await PlayNextTrack();
                }
                _listenForButtons = true;
            }), null);
        }

        /// <summary>
        /// Runs when a track finished playing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">FinishedStoppedPlayingEventArgs.</param>
        private void OnTrackFinished(object sender, FinishedStoppedPlayingEventArgs e)
        {
            // Run in the UI thread.
            _context.Post(new SendOrPostCallback(async (state) =>
            {
                ClearPlayerInfo();
                await GetAndPlayNextTrack();
            }
            ), null);
        }

        /// <summary>
        /// Runs when a track's position has changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">PositionChangedEventArgs.</param>
        private void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            TrackPosition = e.Position;
        }

        /// <summary>
        /// Clears all info on the player control.
        /// </summary>
        public void ClearPlayerInfo()
        {
            Title = String.Empty;
            ThumbnailImageUrl = null;
            TrackPosition = DefaultTrackPosition;

            PlayPauseButtonEnabled = false;
            NextTrackButtonEnabled = false;
        }

        public async void OnPlay()
        {
            if (_listenForButtons)
            {
                _listenForButtons = false;
                await _audioPlayer.PlayAsync();
                PlayButtonVisibility = Visibility.Collapsed;
                PauseButtonVisibility = Visibility.Visible;
                _listenForButtons = true;
            }
            
        }

        public async void OnPause()
        {
            if (_listenForButtons)
            {
                _listenForButtons = false;
                await _audioPlayer.PauseAsync();
                PauseButtonVisibility = Visibility.Collapsed;
                PlayButtonVisibility = Visibility.Visible;
                _listenForButtons = true;
            }
            
        }

        public async void OnNext()
        {
            if (_listenForButtons)
            {
                ClearPlayerInfo();
                _listenForButtons = false;
                await GetAndPlayNextTrack();
                _listenForButtons = true;
            }
        }

        private async Task RefreshDownloadQueue()
        {
            _isRefreshingDownloadQueue = true;
            try
            {
                // We refresh the download queue
                List<string> nextNTrackIds = (await _playlistService.GetNextNItemsAsync(_downloadConfig.DownloadAheadAmount)).Select(playableItemInfo => playableItemInfo.ItemId).ToList();
                await _trackDownloader.SetDownloadQueueAsync(nextNTrackIds);
            }
            catch { }
            _isRefreshingDownloadQueue = false;
        }

        private void OnShutDownMessage(ShutDownMessage shutDownMessage)
        {
            PlayPauseButtonEnabled = false;
            NextTrackButtonEnabled = false;
            _audioPlayer.FinishedPlaying -= OnTrackFinished;
            _audioPlayer.PositionChanged -= OnPositionChanged;
            _trackDownloader.TrackDownloadCompleted -= OnTrackDownloadCompleted;
            _trackDownloader.TrackDownloadFailed -= OnTrackDownloadFailed;
            _refreshDownloadQueueTimer.Stop();
            _refreshDownloadQueueTimer.Dispose();
            _retryTimer.Stop();

            Title = UIStrings.NoTracksToPlay;
            TrackPosition = DefaultTrackPosition;
        }
    }
}
