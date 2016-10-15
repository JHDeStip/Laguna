using System.Windows;

using JhDeStip.Laguna.Player.Utility;

namespace JhDeStip.Laguna.Player
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var locator = new ServiceLocator();

            App.Current.Resources.Add("serviceLocator", locator);

            INavigationService navService = locator.NavigationService;
            navService.Initialize(locator.MainWindowViewModel);
            navService.NavigateTo(ServiceLocator.NextNItemsView);
        }
    }
}
