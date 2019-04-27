using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSecurityApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StreamListManagement : ContentPage
	{
        public ObservableCollection<string> StreamUrl = new ObservableCollection<string>();
        private string Key = "StreamUrl_";
        private int CounterUrl = 0;

		public StreamListManagement ()
		{
			InitializeComponent ();
            LoadStreamList();
        }

        private void LoadStreamList()
        {
            #if RELEASE
            int counter = 0;
            string stringTemp;

            while(Preferences.ContainsKey(Key + Convert.ToString(counter)))
            {
                stringTemp = Preferences.Get(Key + Convert.ToString(counter), string.Empty);
                if(!string.IsNullOrEmpty(stringTemp))
                {
                    StreamUrl.Add(stringTemp);
                }
                counter++;
            }
            CounterUrl = counter;
            #endif

            #if DEBUG
            StreamUrl.Add("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
            StreamUrl.Add("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
            StreamUrl.Add("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
            #endif

            streamList.ItemsSource = StreamUrl;
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            string itemElements = (sender as MenuItem).CommandParameter.ToString();
            int counter = StreamUrl.IndexOf(itemElements);

            #if RELEASE
            Preferences.Set(Key + Convert.ToString(counter), string.Empty);
            #endif

            do
            {
                Preferences.Set(Key + Convert.ToString(counter), Preferences.Get(Key + Convert.ToString(counter + 1), string.Empty));
                counter++;
            }
            while (Preferences.ContainsKey(Key + Convert.ToString(counter + 1)));

            Preferences.Remove(Key + Convert.ToString(counter));
            StreamUrl.Remove((sender as MenuItem).CommandParameter.ToString());
        }

        private void StreamList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            #if RELEASE
            if (!string.IsNullOrEmpty(entryUrl.Text))
                Preferences.Set(Key + CounterUrl, entryUrl.Text);
            #endif

            StreamUrl.Add(entryUrl.Text);
            entryUrl.Text = string.Empty;
        }

        //private void SaveButton_Clicked(object sender, EventArgs e)
        //{
        //    for(int i = 0; i < StreamUrl.Count; i++)
        //    {
        //        //Preferences.Set(Key + Convert.ToString(i), StreamUrl[i]);
        //    }
        //}

        //private void UrlEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}
    }
}