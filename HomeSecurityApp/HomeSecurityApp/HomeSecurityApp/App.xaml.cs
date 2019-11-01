using LibVLCSharp.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
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

            MainPage = new Pages.ShellApp();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            // Check if SingleStreamVisualization Is Open
            if (this.MainPage.Navigation.ModalStack.Count > 0)
            {
                if(this.MainPage.Navigation.ModalStack[0].GetType() == typeof(Pages.SingleCameraVisualization))
                {
                    this.MainPage.Navigation.PopModalAsync(false);
                    //Pages.SingleStreamVisualization singleStreamVisualizationReference = this.MainPage.Navigation.ModalStack[0] as Pages.SingleStreamVisualization;
                    //singleStreamVisualizationReference.MediaPlayerToUse.Stop();
                }
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // Check if SingleStreamVisualization Is Open
            //if (this.MainPage.Navigation.ModalStack.Count > 0)
            //{
            //    if (this.MainPage.Navigation.ModalStack[0].GetType() == typeof(Pages.SingleStreamVisualization))
            //    {
            //    }
            //}
        }
    }
}
