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
	public partial class StreamListManagement : ContentPage
	{
        private List<string> StreamUrl = new List<string>();

		public StreamListManagement ()
		{
			InitializeComponent ();
            LoadStreamList();
		}

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
            StreamUrl.Add("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
            streamList.ItemsSource = StreamUrl;
        }
    }
}