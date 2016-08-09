using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace JhDeStip.Laguna.Client.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            // Hide the navigation bar for this page.
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            BindingContext = App.Locator.AboutPageViewModel;
        }
    }
}
