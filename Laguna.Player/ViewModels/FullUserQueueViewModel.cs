using System.Threading.Tasks;
using System.Windows;
using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using JhDeStip.Laguna.Domain;
using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Player.Messages;
using JhDeStip.Laguna.Player.Resources;
using JhDeStip.Laguna.Player.Utility;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public class FullUserQueueViewModel : ViewModelBase
    {
        #region Instance variables

        private IPlaylistService _playlistService;

        private IDialogService _dialogService;

        #endregion

        #region Commands

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand UnloadedCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand<PlayableItemInfo> RemoveTrackCommand { get; private set; }

        #endregion

        #region Properties for view

        private IList<PlayableItemInfo> _items;
        public IList<PlayableItemInfo> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (value != _items)
                {
                    _items = value;
                    RaisePropertyChanged(nameof(Items));
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

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    RaisePropertyChanged(nameof(Message));
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

        private Visibility _listVisibility;
        public Visibility ListVisibility
        {
            get
            {
                return _listVisibility;
            }
            set
            {
                if (value != _listVisibility)
                {
                    _listVisibility = value;
                    RaisePropertyChanged(nameof(ListVisibility));
                }
            }
        }

        private Visibility _messageVisibility;
        public Visibility MessageVisibility
        {
            get
            {
                return _messageVisibility;
            }
            set
            {
                if (value != _messageVisibility)
                {
                    _messageVisibility = value;
                    RaisePropertyChanged(nameof(MessageVisibility));
                }
            }
        }

        #endregion

        public FullUserQueueViewModel(IPlaylistService playlistService, IDialogService dialogService)
        {
            _playlistService = playlistService;
            _dialogService = dialogService;

            CreateCommands();
        }

        /// <summary>
        /// Creates all commands. It assigns methods to commands.
        /// </summary>
        private void CreateCommands()
        {
            LoadedCommand = new RelayCommand(OnLoaded);
            UnloadedCommand = new RelayCommand(OnUnloaded);
            RefreshCommand = new RelayCommand(OnRefresh);
            RemoveTrackCommand = new RelayCommand<PlayableItemInfo>(RemoveTrack);
        }

        private async Task RefreshList()
        {
            Message = UIStrings.Loading;
            ListVisibility = Visibility.Collapsed;
            MessageVisibility = Visibility.Visible;
            ControlsEnabled = false;

            try
            {
                Items = await _playlistService.GetUserQueueItemsAsync();

                if (Items.Count > 0)
                {
                    MessageVisibility = Visibility.Collapsed;
                    ListVisibility = Visibility.Visible;
                }
                else
                    Message = UIStrings.NoTracksInUserQueue;
            }
            catch (ServerCommunicationException)
            {
                Message = UIStrings.SomethingWentWrong;
                ListVisibility = Visibility.Collapsed;
                MessageVisibility = Visibility.Visible;
            }
            catch
            {
                Message = UIStrings.ServiceCurrentlyNotAvailable;
                ListVisibility = Visibility.Collapsed;
                MessageVisibility = Visibility.Visible;
            }

            ControlsEnabled = true;
        }

        private async void RemoveTrack(PlayableItemInfo playableItemInfo)
        {
            try
            {
                await _playlistService.RemoveUserQueueItemAsync(playableItemInfo);
                await RefreshList();
                Messenger.Default.Send(new RefreshDownloadQueueMessage());
            }
            catch
            {
                _dialogService.ShowErrorMessageBox(UIStrings.DialogTitle_Failed, UIStrings.DialogText_CannotRemoveTrack);
            }
        }

        private async void OnLoaded()
        {
            await RefreshList();
            Messenger.Default.Register<RefreshListMessage>(this, HandleRefreshListMessage);
        }

        private void OnUnloaded()
        {
            Messenger.Default.Unregister<RefreshListMessage>(this, HandleRefreshListMessage);
        }

        private async void OnRefresh()
        {
            await RefreshList();
        }

        private async void HandleRefreshListMessage(RefreshListMessage refreshListMessage)
        {
            await RefreshList();
        }
    }
}
