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

namespace HomeSecurityApp.Pages.Phone
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePagePhone : ContentPage
    {
        private List<string> StreamUrl = new List<string>();
        private List<VideoView> VideoViewList = new List<VideoView>();

        private LibVLC _LibVlc;

        public HomePagePhone()
        {
            InitializeComponent();

            Core.Initialize();

            _LibVlc = new LibVLC();

            LoadStreamList();
            InitializeGrid();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            for(byte i = 0; i < StreamUrl.Count; i++)
            {
                VideoView videoViewToAdd = new VideoView() {  HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                VideoViewList.Add(videoViewToAdd);
                homeGrid.Children.Add(VideoViewList[i]);

                VideoViewList[i].MediaPlayer = new MediaPlayer(_LibVlc) { Media = new Media(_LibVlc, StreamUrl[i], Media.FromType.FromLocation) };
                VideoViewList[i].MediaPlayer.Play();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            foreach(VideoView videowView in VideoViewList)
            {
                videowView.MediaPlayer.Stop();
                videowView.MediaPlayer.Media.Dispose();
            }
        }

        #region Private Method

        private void LoadStreamList()
        {
            //string key = "StreamUrl_";
            //byte counter = 0;
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
        }

        private void InitializeGrid()
        {
            for(byte i = 0; i < 1; i++)
            {
                homeGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            }
        }

        #endregion

        #region Event Handler

        private void AddStream_Clicked(object sender, EventArgs e)
        {
            //overlayAddStreamUrl.IsVisible = true;
        }

        //private void InsertStream_Clicked(object sender, EventArgs e)
        //{
        //    string key = "StreamUrl_";
        //    if(!string.IsNullOrEmpty(StreamUrlEntry.Text))
        //    {
        //        Preferences.Set(key + Convert.ToString(StreamUrl.Count), StreamUrlEntry.Text);
        //    }
        //    StreamUrlEntry.Text = string.Empty;
        //    overlayAddStreamUrl.IsVisible = false;
        //}

        //private void CancelInsertStream_Clicked(object sender, EventArgs e)
        //{
        //    StreamUrlEntry.Text = string.Empty;
        //    overlayAddStreamUrl.IsVisible = false;
        //}

        #endregion
    }
}