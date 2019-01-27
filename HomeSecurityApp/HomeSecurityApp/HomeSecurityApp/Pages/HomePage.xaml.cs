using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LibVLCSharp.Shared;
using LibVLCSharp.Forms.Shared;

namespace HomeSecurityApp.Pages
{
    public partial class HomePage : ContentPage
    {
        List<string> StreamUrl = new List<string>();
        List<VideoView> VideoViewList = new List<VideoView>();

        LibVLC _LibVlc;

        private bool NeedLoad = true;

        public HomePage()
        {
            InitializeComponent();

            LoadStreamList();
            InitializeGrid();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Core.Initialize();

            _LibVlc = new LibVLC();

            for (int i = 0; i < StreamUrl.Count; i++)
            {
                VideoViewList.Add(new VideoView { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HeightRequest = 300, MinimumHeightRequest = 300 });
                homeGrid.Children.Add(VideoViewList[i], 0, i);
                homeGrid.LayoutChanged += HomeGrid_LayoutChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            foreach (VideoView videowView in VideoViewList)
            {
                videowView.MediaPlayer.Stop();
                videowView.MediaPlayer.Media.Dispose();
                //videowView.MediaPlayer = null;
                //videowView.MediaPlayer.Dispose();
            }
            _LibVlc.Dispose();
            NeedLoad = false;
            homeGrid.Children.Clear();
            NeedLoad = true;
        }

        #region Private Method

        private void LoadStreamList()
        {
            //string key = "StreamUrl_";
            //int counter = 0;
            //string stringTemp;

            //while(Preferences.ContainsKey(key + Convert.ToString(counter)))
            //{
            //    stringTemp = Preferences.Get(key + Convert.ToString(counter), string.Empty);
            //    if(!string.IsNullOrEmpty(stringTemp))
            //    {
            //        StreamUrl.Add(stringTemp);
            //    }
            //    counter++;
            //}
            StreamUrl.Add("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
            StreamUrl.Add("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
        }

        private void InitializeGrid()
        {
            homeGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            for (int i = 0; i < 1; i++)
            {
                homeGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
        }

        #endregion

        #region Event Handler

        private void HomeGrid_LayoutChanged(object sender, EventArgs e)
        {
            if(NeedLoad)
            {
                for (int i = 0; i < StreamUrl.Count; i++)
                {
                    VideoViewList[i].MediaPlayer = new MediaPlayer(_LibVlc) { Media = new Media(_LibVlc, StreamUrl[i], Media.FromType.FromLocation), Volume = 0 };
                    VideoViewList[i].MediaPlayer.Play();
                }
            }
        }

        #endregion
    }
}