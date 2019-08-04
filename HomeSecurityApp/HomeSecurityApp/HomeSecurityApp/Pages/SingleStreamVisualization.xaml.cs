using HomeSecurityApp.Utility;
using HomeSecurityApp.Utility.Interface;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSecurityApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingleStreamVisualization : ContentPage
	{
        #region Variables

        public MediaPlayer MediaPlayerToUse;

        #endregion

        #region Constuctor

        public SingleStreamVisualization(MediaPlayer mediaPlayer)
        {
            InitializeComponent();

            if(mediaPlayer != null)
            {
                this.MediaPlayerToUse = mediaPlayer;
                videoViewToDisplay.MediaPlayer = MediaPlayerToUse;
                videoViewToDisplay.MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            }
        }

        #endregion

        #region Overrides Method
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                videoViewToDisplay.MediaPlayer.Fullscreen = true;
                videoViewToDisplay.MediaPlayer.Play();
            }
            catch (Exception ex)
            {
#if DEBUG
                Trace.TraceError($"SingleStreamVisualization - OnAppearing: {ex.Message}");
#endif
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            videoViewToDisplay.MediaPlayer.Stop();
        }

        #endregion

        #region Event Handler

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            try
            {
                videoViewToDisplay.MediaPlayer.Stop();
                videoViewToDisplay.IsVisible = false;

                lInfo.Text = $"{nameof(MediaPlayer_EncounteredError)} of {MediaPlayerToUse.Media.Mrl}";
                lInfo.IsVisible = true;

                Trace.TraceError($"{nameof(MediaPlayer_EncounteredError)} of {videoViewToDisplay.MediaPlayer.Media.Mrl}");
            }
            catch (Exception ex)
            {
#if DEBUG
                Trace.TraceError($"SingleStreamVisualization - MediaPlayer_EncounteredError: {ex.Message}");
#endif
            }
        }

        #endregion

        #region Public Method

        #endregion
    }
}