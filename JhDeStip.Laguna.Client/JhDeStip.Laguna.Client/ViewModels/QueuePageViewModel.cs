using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Domain;

namespace JhDeStip.Laguna.Client.ViewModels
{
    public class QueuePageViewModel : ViewModelBase
    {
        #region Instance variables

        private const string NOTHING_PLAYING_MESSAGE = "Er wordt niks afgespeeld";
        private const string LOADING_MESSAGE = "Laden…";
        private const string SERVICE_NOT_AVAILABLE_MESSAGE = "De dienst is momenteel niet beschikbaar";
        private const string SOMETHING_WENT_WRONG_MESSAGE = "Er is iets misgegaan";
        private const string NOTHING_IN_QUEUE_MESSAGE = "Er staan geen nummers in de wachtrij";

        private IPlaylistService _playlistService;
        private INavigationService _navigationService;

        #endregion

        #region Commands

        public RelayCommand AppearingCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand ToSearchPageCommand { get; private set; }
        public RelayCommand ToAboutPageCommand { get; private set; }
        public RelayCommand ListItemSelectedCommand { get; private set; }

        #endregion

        #region Properties for page

        private bool _fullScreenMessageVisible;
        public bool FullScreenMessageVisible
        {
            get
            {
                return _fullScreenMessageVisible;
            }
            set
            {
                if (value != _fullScreenMessageVisible)
                {
                    _fullScreenMessageVisible = value;
                    RaisePropertyChanged(nameof(FullScreenMessageVisible));
                }
            }
        }
        private bool _nowPlayingVisible;
        public bool NowPlayingVisible
        {
            get
            {
                return _nowPlayingVisible;
            }
            set
            {
                if (value != _nowPlayingVisible)
                {
                    _nowPlayingVisible = value;
                    RaisePropertyChanged(nameof(NowPlayingVisible));
                }
            }
        }
        private bool _queueVisible;
        public bool QueueVisible
        {
            get
            {
                return _queueVisible;
            }
            set
            {
                if (value != _queueVisible)
                {
                    _queueVisible = value;
                    RaisePropertyChanged(nameof(QueueVisible));
                }
            }
        }
        private bool _controlsEnabled;
        public bool ControlsEnabled
        {
            get
            {
                return _controlsEnabled;
            }
            set
            {
                if (value != _controlsEnabled)
                {
                    _controlsEnabled = value;
                    RaisePropertyChanged(nameof(ControlsEnabled));
                }
            }
        }
        private string _fullScreenMessage;
        public string FullScreenMessage
        {
            get
            {
                return _fullScreenMessage;
            }
            set
            {
                if (value != _fullScreenMessage)
                {
                    _fullScreenMessage = value;
                    RaisePropertyChanged(nameof(FullScreenMessage));
                }
            }
        }
        private string _nowPlayingTitle;
        public string NowPlayingTitle
        {
            get
            {
                return _nowPlayingTitle;
            }
            set
            {
                if (value != _nowPlayingTitle)
                {
                    _nowPlayingTitle = value;
                    RaisePropertyChanged(nameof(NowPlayingTitle));
                }
            }
        }
        private string _nowPlayingThumbnailUrl;
        public string NowPlayingThumbnailUrl
        {
            get
            {
                return _nowPlayingThumbnailUrl;
            }
            set
            {
                if (value != _nowPlayingThumbnailUrl)
                {
                    _nowPlayingThumbnailUrl = value;
                    RaisePropertyChanged(nameof(NowPlayingThumbnailUrl));
                }
            }
        }
        private IEnumerable<PlayableItemInfo> _queueItems;
        public IEnumerable<PlayableItemInfo> QueueItems
        {
            get
            {
                return _queueItems;
            }
            set
            {
                if (value != _queueItems)
                {
                    _queueItems = value;
                    RaisePropertyChanged(nameof(QueueItems));
                }
            }
        }

        private PlayableItemInfo _selectedItem;
        public PlayableItemInfo SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        #endregion

        public QueuePageViewModel(IPlaylistService playlistService, INavigationService navigationService)
        {
            _playlistService = playlistService;
            _navigationService = navigationService;

            InitPageProperties();

            CreateCommands();
        }

        /// <summary>
        /// Creates all commands. It assigns methods to commands.
        /// </summary>
        private void CreateCommands()
        {
            AppearingCommand = new RelayCommand(Refresh);
            RefreshCommand = new RelayCommand(Refresh);
            ToSearchPageCommand = new RelayCommand(() =>
            {
                ControlsEnabled = false;
                _navigationService.NavigateTo(ViewModelLocator.SEARCH_PAGE);
            });
            ToAboutPageCommand = new RelayCommand(() =>
            {
                ControlsEnabled = false;
                _navigationService.NavigateTo(ViewModelLocator.ABOUT_PAGE);
            });
            ListItemSelectedCommand = new RelayCommand(() => SelectedItem = null);
        }

        private void InitPageProperties()
        {
            ControlsEnabled = true;
            FullScreenMessageVisible = true;
            NowPlayingVisible = false;
            QueueVisible = false;
            FullScreenMessage = LOADING_MESSAGE;
        }

        private async void Refresh()
        {
            ControlsEnabled = false;
            NowPlayingVisible = false;
            QueueVisible = false;
            FullScreenMessageVisible = true;
            FullScreenMessage = LOADING_MESSAGE;

            try
            {
                var nowPlayingItem = await _playlistService.GetNowPlayingItemAsync();

                if (nowPlayingItem == null)
                {
                    NowPlayingTitle = NOTHING_PLAYING_MESSAGE;
                    NowPlayingThumbnailUrl = null;
                }
                else
                {
                    NowPlayingTitle = nowPlayingItem.Title;
                    NowPlayingThumbnailUrl = nowPlayingItem.ThumbnailUrl;
                }

                QueueItems = await _playlistService.GetUserQueueItemsAsync();

                if (QueueItems.Count() > 0)
                {
                    FullScreenMessageVisible = false;
                    QueueVisible = true;
                }
                else
                    FullScreenMessage = NOTHING_IN_QUEUE_MESSAGE;

                NowPlayingVisible = true;
            }
            catch (ServerCommunicationException)
            {
                FullScreenMessage = SOMETHING_WENT_WRONG_MESSAGE;
            }
            catch
            {
                FullScreenMessage = SERVICE_NOT_AVAILABLE_MESSAGE;
            }

            ControlsEnabled = true;
        }
    }
}