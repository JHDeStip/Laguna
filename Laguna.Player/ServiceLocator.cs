using System.IO;
using Newtonsoft.Json;

using GalaSoft.MvvmLight.Ioc;
using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Player.ViewModels;
using JhDeStip.Laguna.Player.Views;
using JhDeStip.Laguna.Player.Utility;
using JhDeStip.Laguna.Player.Config;
using JhDeStip.Laguna.TrackProvider;
using Stannieman.AudioPlayer;
using Stannieman.CacheTemp;
using JhDeStip.Laguna.Player.Helpers;
using System.Windows;
using JhDeStip.Laguna.Player.Resources;
using System;

namespace JhDeStip.Laguna.Player
{
    /// <summary>
    /// Class to locate view models and services.
    /// </summary>
    public class ServiceLocator
    {
        #region Instance variables and constants

        private const string ConfigFileName = "Config.json";

        private CacheTempConfig _cacheTempConfig;
        private YoutubeDownloadConfig _youtubeDownloadConfig;
        private TrackDownloadConfig _trackDownloadConfig;
        private DalConfig _dalConfig;

        #endregion

        #region View keys

        public const string NextNItemsView = "NextNItemsView";
        public const string FullUserQueueView = "FullUserQueueView";
        public const string FallbackPlaylistView = "FallbackPlaylistView";
        public const string SettingsView = "SettingsView";
        public const string InfoMessageView = "InfoMessageView";

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceLocator()
        {
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            BuildConfiguration();

            //Register services
            SimpleIoc.Default.Register(() => CreateNavigationService());
            SimpleIoc.Default.Register(() => _cacheTempConfig);
            SimpleIoc.Default.Register(() => _youtubeDownloadConfig);
            SimpleIoc.Default.Register(() => _trackDownloadConfig);
            SimpleIoc.Default.Register(() => _dalConfig);
            SimpleIoc.Default.Register<IPlaylistService, PlaylistService>();
            SimpleIoc.Default.Register<ICacheTempManager, CacheTempManager>();
            SimpleIoc.Default.Register<ITrackDownloader, YoutubeTrackDownloader>();
            SimpleIoc.Default.Register<IServiceAvailabilityService, ServiceAvailabilityService>();
            SimpleIoc.Default.Register<IAudioPlayer, AudioPlayer>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            //Register viewmodels
            SimpleIoc.Default.Register<FullUserQueueViewModel>();
            SimpleIoc.Default.Register<NextNItemsViewModel>();
            SimpleIoc.Default.Register<FallbackPlaylistViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<PlayerViewModel>();
            SimpleIoc.Default.Register<InfoMessageViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]

        #region View model properties

        public FullUserQueueViewModel FullUserQueueViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<FullUserQueueViewModel>();
            }
        }

        public NextNItemsViewModel NextNItemsViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NextNItemsViewModel>();
            }
        }

        public FallbackPlaylistViewModel FallbackPlaylistViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<FallbackPlaylistViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<PlayerViewModel>();
            }
        }

        public InfoMessageViewModel InfoMessageViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<InfoMessageViewModel>();
            }
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        #endregion

        #region Service properties

        public INavigationService NavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public IPlaylistService PlaylistService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IPlaylistService>();
            }
        }

        public ICacheTempManager CacheManager
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICacheTempManager>();
            }
        }

        public ITrackDownloader TrackDownloader
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITrackDownloader>();
            }
        }

        public IAudioPlayer AudioPlayer
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IAudioPlayer>();
            }
        }

        public IServiceAvailabilityService ServiceAvailabilityService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IServiceAvailabilityService>();
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IDialogService>();
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Creates and configures a new NavigationService instance.
        /// </summary>
        /// <returns>INavigationService instance.</returns>
        private INavigationService CreateNavigationService()
        {
            var navService = new NavigationService();

            // Register pages to navigation service.
            navService.Configure(NextNItemsView, typeof(NextNItemsView));
            navService.Configure(FullUserQueueView, typeof(FullUserQueueView));
            navService.Configure(FallbackPlaylistView, typeof(FallbackPlaylistView));
            navService.Configure(SettingsView, typeof(SettingsView));
            navService.Configure(InfoMessageView, typeof(InfoMessageView));

            return navService;
        }

        /// <summary>
        /// Creates and populates a configuration classes instance.
        /// </summary>
        private void BuildConfiguration()
        {
            try
            {
                dynamic config = JsonConvert.DeserializeObject(File.ReadAllText(ConfigFileName));

                var configHelper = new ConfigHelper();
                _dalConfig = configHelper.BuildDalConfig(config);
                _cacheTempConfig = configHelper.BuildCacheTempConfig(config);
                _trackDownloadConfig = configHelper.BuildTrackDownloadConfig(config);
                _youtubeDownloadConfig = configHelper.BuildYoutubeDownloadConfig(config);
            }
            catch
            {
#if !DEBUG
                MessageBox.Show(UIStrings.DialogText_MissingOrInvalidValuesInConfiguration, UIStrings.DialogTitle_InvalidConfiguration, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
#endif
            }
        }

        #endregion
    }
}
