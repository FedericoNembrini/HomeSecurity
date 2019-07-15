using HomeSecurityApp.Utility;
using HomeSecurityApp.Utility.Interface;
using LibVLCSharp.Forms.Shared;
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
	public partial class SingleStreamVisualization : ContentPage
	{
        MediaPlayer MediaPlayerToUse;

		public SingleStreamVisualization (MediaPlayer mediaPlayer)
		{
			InitializeComponent();

            this.MediaPlayerToUse = mediaPlayer;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            videoViewToDisplay.MediaPlayer = MediaPlayerToUse;
            videoViewToDisplay.MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            videoViewToDisplay.MediaPlayer.Fullscreen = true;
            videoViewToDisplay.MediaPlayer.Play();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            videoViewToDisplay.MediaPlayer.Stop();
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            Trace.TraceError($"{nameof(MediaPlayer_EncounteredError)} of {videoViewToDisplay.MediaPlayer.Media.Mrl}");

            //await Navigation.PopModalAsync();
        }
    }
}