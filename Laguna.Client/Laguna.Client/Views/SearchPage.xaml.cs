using JhDeStip.Laguna.Client.ViewModels;
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

        public void OnSearchResultListItemTapped(object sender, ItemTappedEventArgs e)
        {

            var viewModel = BindingContext as SearchPageViewModel;
            if (viewModel != null)
            {
                ((ListView)sender).SelectedItem = null;
                viewModel.ItemTappedCommand.Execute(e.Item);
            }
        }
    }
}
