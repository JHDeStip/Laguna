using Xamarin.Forms;

namespace JhDeStip.Laguna.Client.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            // Hide the navigation bar for this page.
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            BindingContext = App.Locator.SearchPageViewModel;
        }
    }
}
