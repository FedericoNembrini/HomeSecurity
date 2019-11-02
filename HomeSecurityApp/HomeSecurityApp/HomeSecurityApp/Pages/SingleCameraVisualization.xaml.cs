using HomeSecurityApp.Utility;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSecurityApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleCameraVisualization : ContentPage
    {
        #region Variables

        LibVLC LibVlc;

        MediaPlayer MediaPlayerToUse;

        CameraObject cameraObject;

        #endregion

        #region Constuctor

        public SingleCameraVisualization(CameraObject cameraObject)
        {
            InitializeComponent();

            this.cameraObject = cameraObject;

            LibVlc = new LibVLC();
            
            this.MediaPlayerToUse = new MediaPlayer(new Media(LibVlc, this.cameraObject.ConnectionUrl, FromType.FromLocation));
            videoViewToDisplay.MediaPlayer = MediaPlayerToUse;
            videoViewToDisplay.MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            videoViewToDisplay.MediaPlayer.Media.StateChanged += Media_StateChanged;
        }

        #endregion

        #region Overrides Method
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None
                    || Connectivity.NetworkAccess == NetworkAccess.Unknown)
                {
                    videoViewToDisplay.IsVisible = false;

                    lInfo.Text = "No Internet Access Found.";
                    lInfo.IsVisible = true;

                    return;
                }

                videoViewToDisplay.MediaPlayer.Fullscreen = true;
                videoViewToDisplay.MediaPlayer.Play();
            }
            catch (Exception ex)
            {
#if DEBUG
                Trace.TraceError($"SingleCameraVisualization - OnAppearing: {ex.Message}");
#endif
            }
        }

        protected override void OnDisappearing()
        {
            videoViewToDisplay.MediaPlayer.Stop();

            base.OnDisappearing();
        }

        #endregion

        #region Event Handler

        private void Media_StateChanged(object sender, MediaStateChangedEventArgs e)
        {
            try
            {
                if (e.State == VLCState.Ended)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        videoViewToDisplay.IsVisible = false;

                        lInfo.Text = $"{cameraObject.Name} no more data received.";
                        lInfo.IsVisible = true;
                    });

                    Trace.TraceInformation($"{nameof(Media_StateChanged)} of {videoViewToDisplay.MediaPlayer.Media.Mrl} changed to {e.State}");
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Trace.TraceError($"SingleCameraVisualization - Media_StateChanged: {ex.Message}");
#endif
            }
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            try
            {
                videoViewToDisplay.MediaPlayer.Pause();
                videoViewToDisplay.IsVisible = false;

                lInfo.Text = $"{nameof(MediaPlayer_EncounteredError)} of {MediaPlayerToUse.Media.Mrl}";
                lInfo.IsVisible = true;

                Trace.TraceError($"{nameof(MediaPlayer_EncounteredError)} of {videoViewToDisplay.MediaPlayer.Media.Mrl}");
            }
            catch (Exception ex)
            {
#if DEBUG
                Trace.TraceError($"SingleCameraVisualization - MediaPlayer_EncounteredError: {ex.Message}");
#endif
            }
        }

        #endregion

        #region Public Method

        #endregion
    }
}