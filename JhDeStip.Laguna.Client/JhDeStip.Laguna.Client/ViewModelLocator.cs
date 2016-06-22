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
    public class ViewModelLocator
    {
        #region Page keys

        public const string SEARCH_PAGE = "SearchPage";
        public const string ABOUT_PAGE = "AboutPage";

        #endregion

        #region Instance variables

        private INavigationService _navigationService;
        private DalConfig _dalConfig;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

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
                return ServiceLocator.Current.GetInstance<SearchPageViewModel>();
            }
        }
        public QueuePageViewModel QueuePageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<QueuePageViewModel>();
            }
        }
        public AboutPageViewModel AboutPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutPageViewModel>();
            }
        }

        #endregion

        #region Service properties

        public INavigationService INavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public IDialogService IDialogService
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
            navService.Configure(SEARCH_PAGE, typeof(SearchPage));
            navService.Configure(ABOUT_PAGE, typeof(AboutPage));

            return navService;
        }

        #endregion
    }
}
