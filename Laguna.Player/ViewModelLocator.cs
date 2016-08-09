using Microsoft.Practices.ServiceLocation;
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

namespace JhDeStip.Laguna.Player
{
    /// <summary>
    /// Class to locate view models and services.
    /// </summary>
    public class ViewModelLocator
    {
        #region Instance variables and constants

        private const string ConfigFileName = "Config.json";

        private CacheTempConfig _cacheTempConfig;
        private YoutubeDownloadConfig _youtubeDownloadConfig;
        private TrackDownloadConfig _trackDownloadConfig;
        private DalConfig _dalConfig;

        #endregion

        #region View keys

        public const string NEXT_N_ITEMS_VIEW_KEY = "NextNItemsView";
        public const string FULL_USER_QUEUE_VIEW_KEY = "FullUserQueueView";
        public const string FALLBACK_PLAYLIST_VIEW_KEY = "FallbackPlaylistView";
        public const string SETTINGS_VIEW_KEY = "SettingsView";
        public const string INFO_MESSAGE_VIEW_KEY = "InfoMessageView";

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

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
                return ServiceLocator.Current.GetInstance<FullUserQueueViewModel>();
            }
        }

        public NextNItemsViewModel NextNItemsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NextNItemsViewModel>();
            }
        }

        public FallbackPlaylistViewModel FallbackPlaylistViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FallbackPlaylistViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayerViewModel>();
            }
        }

        public InfoMessageViewModel InfoMessageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InfoMessageViewModel>();
            }
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        #endregion

        #region Service properties

        public INavigationService NavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public IPlaylistService PlaylistService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IPlaylistService>();
            }
        }

        public ICacheTempManager CacheManager
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ICacheTempManager>();
            }
        }

        public ITrackDownloader TrackDownloader
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ITrackDownloader>();
            }
        }

        public IAudioPlayer AudioPlayer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IAudioPlayer>();
            }
        }

        public IServiceAvailabilityService ServiceAvailabilityService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IServiceAvailabilityService>();
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogService>();
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
            navService.Configure(NEXT_N_ITEMS_VIEW_KEY, typeof(NextNItemsView));
            navService.Configure(FULL_USER_QUEUE_VIEW_KEY, typeof(FullUserQueueView));
            navService.Configure(FALLBACK_PLAYLIST_VIEW_KEY, typeof(FallbackPlaylistView));
            navService.Configure(SETTINGS_VIEW_KEY, typeof(SettingsView));
            navService.Configure(INFO_MESSAGE_VIEW_KEY, typeof(InfoMessageView));

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
            catch { }
        }

        #endregion
    }
}
