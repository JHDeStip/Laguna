using Xamarin.Forms;
using JhDeStip.Laguna.Client.ViewModels;

namespace JhDeStip.Laguna.Client.Views
{
    public partial class QueuePage : ContentPage
    {
        public QueuePage()
        {
            // Hide the navigation bar for this page.
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            BindingContext = App.Locator.QueuePageViewModel;
        }
    }
}
