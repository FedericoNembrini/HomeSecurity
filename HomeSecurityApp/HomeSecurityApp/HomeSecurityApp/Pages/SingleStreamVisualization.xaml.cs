using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
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
        LibVLC _LibVlc;
        string VideoUrlToDisplay;
		public SingleStreamVisualization (string videoUrlToDisplay)
		{
			InitializeComponent ();
            
            this.VideoUrlToDisplay = videoUrlToDisplay;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _LibVlc = new LibVLC();

            videoViewToDisplay.MediaPlayer = new MediaPlayer(_LibVlc) { Media = new Media(_LibVlc, VideoUrlToDisplay, Media.FromType.FromLocation), Volume = 0, Fullscreen = true };
            videoViewToDisplay.MediaPlayer.Play();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            videoViewToDisplay.MediaPlayer.Stop();
            videoViewToDisplay.MediaPlayer.Media.Dispose();
            _LibVlc.Dispose();
        }
    }
}