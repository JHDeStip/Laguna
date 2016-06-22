using Android.App;
using Android.Content;

namespace JhDeStip.Laguna.Client.Droid
{
    [Activity(Theme = "@style/Theme.SplashScreen", Label = "Laguna Preview 4", Icon = "@drawable/Icon", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}