using HomeSecurityApp.Utility;
using HomeSecurityApp.Utility.Interface;
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
            videoViewToDisplay.MediaPlayer.Fullscreen = true;
            videoViewToDisplay.MediaPlayer.Play();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            videoViewToDisplay.MediaPlayer.Stop();
        }
    }
}