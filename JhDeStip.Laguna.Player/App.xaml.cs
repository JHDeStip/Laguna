using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration.Assemblies;

using GalaSoft.MvvmLight.Messaging;

using JhDeStip.Laguna.Player.Utility;
using JhDeStip.Laguna.Player.ViewModels;
using JhDeStip.Laguna.Player.Messages;

namespace JhDeStip.Laguna.Player
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var locator = (ViewModelLocator)Current.FindResource("viewModelLocator");

            INavigationService navService = locator.NavigationService;
            navService.Initialize(locator.MainWindowViewModel);
            navService.NavigateTo(ViewModelLocator.NEXT_N_ITEMS_VIEW_KEY);
        }
    }
}
