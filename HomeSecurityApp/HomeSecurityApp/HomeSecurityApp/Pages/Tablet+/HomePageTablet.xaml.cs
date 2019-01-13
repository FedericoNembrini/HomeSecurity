using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;

namespace HomeSecurityApp.Pages.Tablet_
{
    public partial class HomePageTablet : ContentPage
    {
        readonly LibVLC _libVlc;
        VideoView videoView1; 
        const string VIDEO_URL = "rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov";


        public HomePageTablet()
        {
            InitializeComponent();

            Core.Initialize();

            _libVlc = new LibVLC();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            videoView1 = new VideoView();
            videoView1.MediaPlayer = new MediaPlayer(_libVlc);

            //videoView1.BackgroundColor = Color.Transparent;
            //videoView1.VerticalOptions = LayoutOptions.Fill;
            //videoView1.HorizontalOptions = LayoutOptions.Fill;

            //videoView1.Loaded += VideoView1_Loaded;

            homeGrid.Children.Add(videoView1, 0, 0);
            videoView1.MediaPlayer.Play(new Media(_libVlc, VIDEO_URL, Media.FromType.FromLocation));
        }

        //private void VideoView1_Loaded(object sender, EventArgs e)
        //{
        //    videoView1.MediaPlayer.Play();
        //}
    }
}