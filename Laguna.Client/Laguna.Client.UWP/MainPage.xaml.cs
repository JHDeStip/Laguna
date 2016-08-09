using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace JhDeStip.Laguna.Client.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = new Color() { R = 231, G = 48, B = 41 } ;
                    statusBar.ForegroundColor = Colors.White;
                }
            }

            LoadApplication(new JhDeStip.Laguna.Client.App());
        }
    }
}
