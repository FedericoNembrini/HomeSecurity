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
using System.Diagnostics;

namespace HomeSecurityApp.Pages
{
    public partial class HomePage : ContentPage
    {
        List<string> StreamUrl = new List<string>();
        List<VideoView> VideoViewList = new List<VideoView>();

        LibVLC _LibVlc;

        SingleStreamVisualization singleStreamVisualization;

        private bool NeedLoad = true;

        public HomePage()
        {
            InitializeComponent();
            Application.Current.ModalPushing += Modal_ModalPushing; ;
            Application.Current.ModalPopped += Modal_ModalPopped;

            LoadStreamList();
            InitializeGrid();

            Core.Initialize();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                _LibVlc = new LibVLC();

                TapGestureRecognizer tapGestureToAdd = new TapGestureRecognizer();
                tapGestureToAdd.Tapped += VideoView_TappedAsync;
                homeGrid.LayoutChanged += HomeGrid_LayoutChanged;
                for (int i = 0; i < StreamUrl.Count; i++)
                {
                    VideoView videoToAdd = new VideoView { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HeightRequest = 360, MinimumHeightRequest = 360 };
                    videoToAdd.GestureRecognizers.Add(tapGestureToAdd);
                    VideoViewList.Add(videoToAdd);
                    homeGrid.Children.Add(VideoViewList[i], Device.Idiom == TargetIdiom.Tablet && i % 2 != 0 ? 1 : 0, i);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                foreach (VideoView videowView in VideoViewList)
                {
                    videowView.MediaPlayer.Stop();
                    videowView.MediaPlayer.Media.Dispose();
                }
                _LibVlc.Dispose();
                NeedLoad = false;
                homeGrid.LayoutChanged -= HomeGrid_LayoutChanged;
                homeGrid.Children.Clear();
                VideoViewList = new List<VideoView>();
                NeedLoad = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
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
            if(Device.Idiom == TargetIdiom.Tablet)
                homeGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            for (int i = 0; i < StreamUrl.Count; i++)
            {
                homeGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
        }

        #endregion

        #region Event Handler

        private void HomeGrid_LayoutChanged(object sender, EventArgs e)
        {
            if(NeedLoad && VideoViewList.Count == StreamUrl.Count)
            {
                for (int i = 0; i < StreamUrl.Count; i++)
                {
                    VideoViewList[i].MediaPlayer = new MediaPlayer(_LibVlc) { Media = new Media(_LibVlc, StreamUrl[i], Media.FromType.FromLocation), Volume = 0 };
                    VideoViewList[i].MediaPlayer.Play();
                }
            }
        }

        private async void VideoView_TappedAsync(object sender, EventArgs e)
        {
            try
            {
                singleStreamVisualization = new SingleStreamVisualization((sender as VideoView).MediaPlayer.Media.Mrl);
                OnDisappearing();
                await Navigation.PushModalAsync(singleStreamVisualization);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void Modal_ModalPopped(object sender, ModalPoppedEventArgs e)
        {
            if(e.Modal == singleStreamVisualization)
            {
                OnAppearing();
            }
        }

        private void Modal_ModalPushing(object sender, ModalPushingEventArgs e)
        {
            if(e.Modal == singleStreamVisualization)
            {
                OnDisappearing();
            }
        }

        #endregion
    }
}