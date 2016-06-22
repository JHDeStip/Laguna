using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using JhDeStip.Laguna.Domain;
using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Player.Messages;
using JhDeStip.Laguna.Player.Resources;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public class FallbackPlaylistViewModel : ViewModelBase
    {
        #region Instance variables and constants

        private IPlaylistService _playlistService;

        #endregion

        #region Commands

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand UnloadedCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }

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

        public FallbackPlaylistViewModel(IPlaylistService playlistService)
        {
            _playlistService = playlistService;

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
        }

        private async Task RefreshList()
        {
            Message = UiStrings.Loading;
            ListVisibility = Visibility.Collapsed;
            MessageVisibility = Visibility.Visible;
            ControlsEnabled = false;

            try
            {
                Items = await _playlistService.GetFallbackPlaylistItemsAsync();
                
                if (Items.Count > 0)
                {
                    MessageVisibility = Visibility.Collapsed;
                    ListVisibility = Visibility.Visible;
                }
                else
                    Message = UiStrings.NoTracksInPlaylist;
            }
            catch (ServerCommunicationException)
            {
                Message = UiStrings.SomethingWentWrong;
                ListVisibility = Visibility.Collapsed;
                MessageVisibility = Visibility.Visible;
            }
            catch
            {
                Message = UiStrings.ServiceCurrentlyNotAvailable;
                ListVisibility = Visibility.Collapsed;
                MessageVisibility = Visibility.Visible;
            }

            ControlsEnabled = true;
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
