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
        LibVLC _libVlc;
        VideoView videoView1;
        MediaPlayer _mediaPlayer;
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

            _mediaPlayer = new MediaPlayer(_libVlc)
            {
                Media = new Media(_libVlc,
                "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4",
                Media.FromType.FromLocation)
            };

            videoView1 = new VideoView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            homeGrid.Children.Add(videoView1);
            
            videoView1.Loaded += VideoView_Loaded;

            videoView1.MediaPlayer = _mediaPlayer;
        }

        private void VideoView_Loaded(object sender, System.EventArgs e)
        {
            //_mediaPlayer.SetAudioTrack(-1);
            _mediaPlayer.Play();
        }

    }
}