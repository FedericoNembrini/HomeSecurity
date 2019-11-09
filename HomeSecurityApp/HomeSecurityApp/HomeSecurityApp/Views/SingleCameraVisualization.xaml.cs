using HomeSecurityApp.Utility;
using HomeSecurityApp.ViewModels;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSecurityApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleCameraVisualization : ContentPage
    {
        #region Variables

        private SingleCameraVisualizationViewModel ViewModel;

        #endregion

        #region Constuctor

        public SingleCameraVisualization(CameraObjectViewModel cameraObject)
        {
            InitializeComponent();

            BindingContext = ViewModel = new SingleCameraVisualizationViewModel(cameraObject);
        }

        #endregion

        #region Overrides Method
        protected override void OnAppearing()
        {
//            base.OnAppearing();

//            try
//            {
//                videoViewToDisplay.MediaPlayer.Fullscreen = true;
//                videoViewToDisplay.MediaPlayer.Play();
//            }
//            catch (Exception ex)
//            {
//#if DEBUG
//                Trace.TraceError($"SingleCameraVisualization - OnAppearing: {ex.Message}");
//#endif
//            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.StopView();
        }

        #endregion

        #region Event Handler

        private void VideoView_MediaPlayerChanged(object sender, MediaPlayerChangedEventArgs e)
        {
            ViewModel.StartView();
        }

        #endregion

        #region Public Method

        #endregion
    }
}