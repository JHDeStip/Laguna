using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Player.Resources;
using JhDeStip.Laguna.Player.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Instance variables and constants

        private IServiceAvailabilityService _serviceAvailabilityService;
        private IPlaylistService _playlistService;
        private IDialogService _dialogService;

        #endregion

        #region Commands

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand RefreshFallbackPlaylistCommand { get; private set; }
        public RelayCommand<bool> MakeServiceAvailableCommand { get; private set; }

        #endregion

        #region Properties for view

        private string _serviceAvailabilityMessage;
        public string ServiceAvailabilityMessage
        {
            get
            {
                return _serviceAvailabilityMessage;
            }
            set
            {
                if (value != _serviceAvailabilityMessage)
                {
                    _serviceAvailabilityMessage = value;
                    RaisePropertyChanged(nameof(ServiceAvailabilityMessage));
                }
            }
        }

        private bool _refreshFallbackPlaylistButtonEnabled;
        public bool RefreshFallbackPlaylistButtonEnabled
        {
            get
            {
                return _refreshFallbackPlaylistButtonEnabled;
            }
            set
            {
                if (value != _refreshFallbackPlaylistButtonEnabled)
                {
                    _refreshFallbackPlaylistButtonEnabled = value;
                    RaisePropertyChanged(nameof(RefreshFallbackPlaylistButtonEnabled));
                }
            }
        }

        private bool _applicationOnOffButtonsEnabled;
        public bool ApplicationOnOffButtonsEnabled
        {
            get
            {
                return _applicationOnOffButtonsEnabled;
            }
            set
            {
                if (value != _applicationOnOffButtonsEnabled)
                {
                    _applicationOnOffButtonsEnabled = value;
                    RaisePropertyChanged(nameof(ApplicationOnOffButtonsEnabled));
                }
            }
        }

        private Visibility _applicationOnButtonVisibility;
        public Visibility ApplicationOnButtonVisibility
        {
            get
            {
                return _applicationOnButtonVisibility;
            }
            set
            {
                if (value != _applicationOnButtonVisibility)
                {
                    _applicationOnButtonVisibility = value;
                    RaisePropertyChanged(nameof(ApplicationOnButtonVisibility));
                }
            }
        }

        private Visibility _applicationOffButtonVisibility;
        public Visibility ApplicationOffButtonVisibility
        {
            get
            {
                return _applicationOffButtonVisibility;
            }
            set
            {
                if (value != _applicationOffButtonVisibility)
                {
                    _applicationOffButtonVisibility = value;
                    RaisePropertyChanged(nameof(ApplicationOffButtonVisibility));
                }
            }
        }

        #endregion

        public SettingsViewModel(IServiceAvailabilityService serviceAvailabilityService, IPlaylistService playlistService, IDialogService dialogService)
        {
            _serviceAvailabilityService = serviceAvailabilityService;
            _playlistService = playlistService;
            _dialogService = dialogService;

            CreateCommands();

            RefreshFallbackPlaylistButtonEnabled = true;
        }

        /// <summary>
        /// Creates all commands. It assigns methods to commands.
        /// </summary>
        private void CreateCommands()
        {
            LoadedCommand = new RelayCommand(OnLoaded);
            RefreshFallbackPlaylistCommand = new RelayCommand(RefreshFallbackPlaylist);
            MakeServiceAvailableCommand = new RelayCommand<bool>(MakeServiceAvailable);
        }

        private async void OnLoaded()
        {
            ApplicationOnOffButtonsEnabled = false;

            ApplicationOnButtonVisibility = Visibility.Collapsed;
            ApplicationOffButtonVisibility = Visibility.Visible;

            try
            {
                if (await _serviceAvailabilityService.IsServiceAvailable())
                {
                    ServiceAvailabilityMessage = UIStrings.ApplicationIsAvailable;
                }
                else
                {
                    ApplicationOffButtonVisibility = Visibility.Collapsed;
                    ApplicationOnButtonVisibility = Visibility.Visible;
                    ServiceAvailabilityMessage = UIStrings.ApplicationIsNotAvailable;
                }

                ApplicationOnOffButtonsEnabled = true;
            }
            catch
            {
                ServiceAvailabilityMessage = UIStrings.CannotRetrieveServiceAvailability;
            }
        }

        private async void RefreshFallbackPlaylist()
        {
            RefreshFallbackPlaylistButtonEnabled = false;

            try
            {
                await _playlistService.RefreshFallbackPlaylistOnServerAsync();
            }
            catch
            {
                _dialogService.ShowErrorMessageBox(UIStrings.DialogTitle_Failed, UIStrings.DialogText_CannotRefreshFallbackPlaylistOnServer);
            }

            RefreshFallbackPlaylistButtonEnabled = true;
        }

        private async void MakeServiceAvailable(bool serviceAvailable)
        {
            ApplicationOnOffButtonsEnabled = false;

            try
            {
                if (serviceAvailable)
                {
                    await _serviceAvailabilityService.MakeServiceAvailable();
                    ApplicationOnButtonVisibility = Visibility.Collapsed;
                    ApplicationOffButtonVisibility = Visibility.Visible;
                    ServiceAvailabilityMessage = UIStrings.ApplicationIsAvailable;
                }
                else
                {
                    await _serviceAvailabilityService.MakeServiceUnavailable();
                    ApplicationOffButtonVisibility = Visibility.Collapsed;
                    ApplicationOnButtonVisibility = Visibility.Visible;
                    ServiceAvailabilityMessage = UIStrings.ApplicationIsNotAvailable;
                }
            }
            catch
            {
                if (serviceAvailable)
                    _dialogService.ShowErrorMessageBox(UIStrings.DialogTitle_Failed, UIStrings.DialogText_CannotMakeServiceAvailable);
                else
                    _dialogService.ShowErrorMessageBox(UIStrings.DialogTitle_Failed, UIStrings.DialogText_CannotMakeServiceUnavailable);
            }

            ApplicationOnOffButtonsEnabled = true;
        }
    }
}
