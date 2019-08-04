using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace HomeSecurityApp.Droid
{
    [Activity(Icon = "@drawable/icon_rectangle", Theme = "@style/Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            await Task.Delay(800);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}