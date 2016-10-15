using Microsoft.Practices.ServiceLocation;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

using JhDeStip.Laguna.Dal;
using JhDeStip.Laguna.Client.ViewModels;
using JhDeStip.Laguna.Client.Utility;
using JhDeStip.Laguna.Client.Views;
using JhDeStip.Laguna.Client.Helpers;

namespace JhDeStip.Laguna.Client
{
    /// <summary>
    /// Class to locate view models and services.
    /// </summary>
    public class ServiceLocator
    {
        #region Page keys

        public const string SearchPage = "SearchPage";
        public const string AboutPage = "AboutPage";

        #endregion

        #region Instance variables

        private INavigationService _navigationService;
        private DalConfig _dalConfig;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceLocator()
        {
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            _dalConfig = new ConfigHelper().BuildDalConfig();

            // Create new NavigationService instance
            _navigationService = CreateNavigationService();

            //Register services
            SimpleIoc.Default.Register(() => _dalConfig);
            SimpleIoc.Default.Register<ITrackSearchService, TrackSearchService>();
            SimpleIoc.Default.Register<IPlaylistService, PlaylistService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register(() => _navigationService);

            //Register viewmodels
            SimpleIoc.Default.Register<QueuePageViewModel>();
            SimpleIoc.Default.Register<SearchPageViewModel>();
            SimpleIoc.Default.Register<AboutPageViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]

        #region View model properties

        public SearchPageViewModel SearchPageViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<SearchPageViewModel>();
            }
        }
        public QueuePageViewModel QueuePageViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<QueuePageViewModel>();
            }
        }
        public AboutPageViewModel AboutPageViewModel
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<AboutPageViewModel>();
            }
        }

        #endregion

        #region Service properties

        public INavigationService INavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public IDialogService IDialogService
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
            navService.Configure(SearchPage, typeof(SearchPage));
            navService.Configure(AboutPage, typeof(AboutPage));

            return navService;
        }

        #endregion
    }
}
