using System;
using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Domain;
using JhDeStip.Laguna.Client.Resources;

namespace JhDeStip.Laguna.Client.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        #region Instance variables

        private ITrackSearchService _trackSearchService;
        private IPlaylistService _playlistService;
        private IDialogService _dialogService;

        #endregion

        #region Commands

        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<PlayableItemInfo> ItemTappedCommand { get; private set; }

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

        private bool _searchResultListVisible;
        public bool SearchResultListVisible
        {
            get
            {
                return _searchResultListVisible;
            }
            set
            {
                if (value != _searchResultListVisible)
                {
                    _searchResultListVisible = value;
                    RaisePropertyChanged(nameof(SearchResultListVisible));
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

        private IEnumerable<PlayableItemInfo> _searchResultItems;
        public IEnumerable<PlayableItemInfo> SearchResultItems
        {
            get
            {
                return _searchResultItems;
            }
            set
            {
                if (value != _searchResultItems)
                {
                    _searchResultItems = value;
                    RaisePropertyChanged(nameof(SearchResultItems));
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

        public SearchPageViewModel(ITrackSearchService trackSearchService, IPlaylistService playlistService, IDialogService dialogService)
        {
            _trackSearchService = trackSearchService;
            _playlistService = playlistService;
            _dialogService = dialogService;

            InitPageProperties();

            CreateCommands();
        }

        /// <summary>
        /// Creates all commands. It assigns methods to commands.
        /// </summary>
        private void CreateCommands()
        {
            SearchCommand = new RelayCommand<string>(OnSearch);
            ItemTappedCommand = new RelayCommand<PlayableItemInfo>(OnItemTapped);
        }

        private void InitPageProperties()
        {
            ControlsEnabled = true;
            FullScreenMessageVisible = true;
            SearchResultListVisible = false;
            FullScreenMessage = UiStrings.EnterSearchTerm;
        }

        public async void OnSearch(string query)
        {
            ControlsEnabled = false;
            SearchResultListVisible = false;
            FullScreenMessageVisible = true;
            FullScreenMessage = UiStrings.Searching;

            try
            {
                SearchResultItems = await _trackSearchService.SearchItemsAsync(query);
                if (SearchResultItems.Count() > 0)
                {
                    FullScreenMessageVisible = false;
                    SearchResultListVisible = true;
                }
                else
                    FullScreenMessage = UiStrings.NoResults;
            }
            catch (ServerCommunicationException)
            {
                FullScreenMessage = UiStrings.SomethingWentWrong;
            }
            catch
            {
                FullScreenMessage = UiStrings.ServiceCurrentlyNotAvailable;
            }
            ControlsEnabled = true;
        }

        private async void OnItemTapped(PlayableItemInfo playableItemInfo)
        {
            ControlsEnabled = false;

            SelectedItem = null;

            if (await _dialogService.ShowMessage(String.Format(UiStrings.QDoYouWantToAddTrackToQueue, playableItemInfo.Title), UiStrings.QAddTrack, UiStrings.Yes, UiStrings.No, null))
            {
                try
                {
                    await _playlistService.AddItemToQueueAsync(playableItemInfo);
                    await _dialogService.ShowMessage(UiStrings.YourTrackIsQueuedToPlay, UiStrings.TrackAdded);
                }
                catch (TrackAlreadyInQueueException)
                {
                    await _dialogService.ShowMessage(UiStrings.TrackAlreadyInQueuePatience, UiStrings.TrackAlreadyInQueue);
                }
                catch (ServerCommunicationException)
                {
                    await _dialogService.ShowMessage(UiStrings.SomethingWentWrongCouldNotAddTrackToQueueKissFromBartender, UiStrings.Error);
                }
                catch
                {
                    await _dialogService.ShowMessage(UiStrings.ServiceCurrentlyNotAvailable, UiStrings.ServiceNotAvailable);
                }
            }

            ControlsEnabled = true;
        }
    }
}