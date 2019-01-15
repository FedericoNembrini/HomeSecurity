using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HomeSecurityApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            switch(Device.Idiom)
            {
                case TargetIdiom.Phone:
                    MainPage = new NavigationPage(new Pages.Phone.HomePagePhone());
                    break;
                case TargetIdiom.Tablet:
                    MainPage = new NavigationPage(new Pages.Tablet_.HomePageTablet());
                    break;
                default:
                    MainPage = new MainPage();
                    break;
            }
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
