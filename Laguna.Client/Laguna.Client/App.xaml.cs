using Xamarin.Forms;
using JhDeStip.Laguna.Client.Utility;
using JhDeStip.Laguna.Client.Views;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace JhDeStip.Laguna.Client
{
    public partial class App : Application
    {
        #region Properties

        private static ServiceLocator _locator;

        public static ServiceLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ServiceLocator());
            }
        }

        #endregion

        public App()
        {
            InitializeComponent();

            // Gets the root page of the app and sets it as navigation in the navigation service.
            var mainPage = GetMainPage();
            var navServ = Locator.INavigationService;
            ((NavigationService)navServ).Initialize((NavigationPage)mainPage);
            var dialogServ = Locator.IDialogService;
            ((DialogService)dialogServ).Initialize((NavigationPage)mainPage);

            // Assign the root page to the MainPage property
            MainPage = mainPage;
        }

        /// <summary>
        /// Returns the root page of the app.
        /// </summary>
        /// <returns>Page instance that's the root page of the app.</returns>
        public static Page GetMainPage()
        {
            return new NavigationPage(new QueuePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}