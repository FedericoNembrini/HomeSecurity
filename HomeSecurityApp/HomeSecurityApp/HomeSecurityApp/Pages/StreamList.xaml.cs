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
using HomeSecurityApp.Utility;
using System.Collections.ObjectModel;
using HomeSecurityApp.Utility.Interface;

namespace HomeSecurityApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StreamList : ContentPage
    {
        #region Variables

        LibVLC _LibVlc;
        public ObservableCollection<StreamListObject> StreamObjectList { get; set; } = new ObservableCollection<StreamListObject>();

        #endregion

        #region Constructors

        public StreamList()
        {
            InitializeComponent();
            Core.Initialize();
        }

        #endregion

        #region Oerride Method

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                LoadStreamList();
                lvStreamList.ItemsSource = StreamObjectList;
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
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Private Method

        private void LoadStreamList()
        {
            _LibVlc = new LibVLC();

            StreamObjectList.Clear();
            #if Release
            int counter = 0;
            string stringTemp;

            while(Preferences.ContainsKey(Utility.Utility.Key + Convert.ToString(counter)))
            {
                stringTemp = Preferences.Get(Utility.Utility.Key + Convert.ToString(counter), string.Empty);
                if(!string.IsNullOrEmpty(stringTemp))
                {
                    StreamObjectList.Add(new StreamListObject(stringTemp, _LibVlc));
                }
                counter++;
            }
            #endif

            StreamObjectList.Add(new StreamListObject("Test#rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175", _LibVlc));
            StreamObjectList.Add(new StreamListObject("Test#rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov", _LibVlc));
        }

        #endregion

        #region Event Handler

        #endregion

        private async void LvStreamList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                #if DEBUG
                    DependencyService.Get<IMessage>().ShortAlert("ListItem Tapped");
                #endif
                await Navigation.PushModalAsync(new SingleStreamVisualization((e.Item as StreamListObject).MediaPlayer));
            }
            catch (Exception ex)
            {
                #if DEBUG
                    DependencyService.Get<IMessage>().LongAlert(ex.Message);
                #endif
            }
        }

        //private void LvStreamList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    StreamListObject item = e.Item as StreamListObject;
        //    item.MediaPlayer.Play();
        //    item.Status = item.MediaPlayer.IsPlaying;
        //}
    }
}