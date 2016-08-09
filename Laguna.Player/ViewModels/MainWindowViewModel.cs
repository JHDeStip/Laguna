using System.Windows.Controls;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using JhDeStip.Laguna.Player.Messages;
using JhDeStip.Laguna.Player.Utility;
using System.Threading.Tasks;
using System.ComponentModel;
using JhDeStip.Laguna.Player.Resources;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INavigator
    {
        #region Instance variables and constants

        private INavigationService _navigationService;
        private string _lastViewKey;
        private bool _busyClosing;

        #endregion

        #region Properties for view

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != null)
                {
                    if (_currentView != value)
                    {
                        if (_currentView.GetType() != value.GetType())
                        {
                            SetNavigationButtonHighLight();
                        }

                        _currentView = value;
                        RaisePropertyChanged(nameof(CurrentView));
                    }
                }
                else
                {
                    _currentView = value;
                    RaisePropertyChanged(nameof(CurrentView));
                }
            }
        }

        private bool _navigationBarEnabled;
        public bool NavigationBarEnabled
        {
            get { return _navigationBarEnabled; }
            set
            {
                if (value != _navigationBarEnabled)
                {
                    _navigationBarEnabled = value;
                    RaisePropertyChanged(nameof(NavigationBarEnabled));
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand<string> NavigateCommand { get; private set; }
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

        #endregion

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _busyClosing = false;

            NavigationBarEnabled = true;

            CreateCommands();

            RegisterForMessages();
        }

        /// <summary>
        /// Creates all commands. It assigns methods to commands.
        /// </summary>
        private void CreateCommands()
        {
            NavigateCommand = new RelayCommand<string>(OnNavigate);
            ClosingCommand = new RelayCommand<CancelEventArgs>(OnClosing);
        }

        /// <summary>
        /// Registers for messages.
        /// </summary>
        private void RegisterForMessages()
        {
            Messenger.Default.Register<ShowInfoMessageMessage>(this, OnShowInfoMessage);
            Messenger.Default.Register<HideInfoMessageMessage>(this, OnHideInfoMessage);
        }

        private void SetNavigationButtonHighLight()
        {
            ;
        }

        private void OnNavigate(string viewKey)
        {
            _navigationService.NavigateTo(viewKey);
            _lastViewKey = viewKey;
        }

        private void OnShowInfoMessage(ShowInfoMessageMessage showInfoMessageMessage)
        {
            _navigationService.NavigateTo(ViewModelLocator.INFO_MESSAGE_VIEW_KEY, showInfoMessageMessage.Message);
            NavigationBarEnabled = false;
        }

        private void OnHideInfoMessage(HideInfoMessageMessage hideInfoMessageMessage)
        {
            if (_lastViewKey != null)
            {
                _navigationService.NavigateTo(_lastViewKey);
                NavigationBarEnabled = true;
            }
        }

        private async void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            if (!_busyClosing)
            {
                _busyClosing = true;

                Messenger.Default.Send(new ShutDownMessage());

                Messenger.Default.Send(new ShowInfoMessageMessage(UiStrings.CleaningUpCacheAndTempFiles));

                await Task.Run(() =>
                {
                    var locator = (ViewModelLocator)App.Current.FindResource("viewModelLocator");

                    locator.AudioPlayer.Dispose();
                    locator.PlaylistService.Dispose();
                    locator.TrackDownloader.Dispose();
                    locator.ServiceAvailabilityService.Dispose();
                    var cacheManager = locator.CacheManager;
                    cacheManager.CleanupCacheAsync();
                    cacheManager.CleanupTempAsync();
                });

                App.Current.Shutdown();
            }
        }
    }
}
