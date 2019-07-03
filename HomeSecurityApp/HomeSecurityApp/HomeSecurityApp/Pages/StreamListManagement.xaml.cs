using HomeSecurityApp.Utility;
using HomeSecurityApp.Utility.Interface;
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
        #region Variables
        public ObservableCollection<string> StreamUrl { get; set; } = new ObservableCollection<string>();

        private int CounterUrl = 0;

        #endregion

        #region Constructor

        public StreamListManagement()
        {
            InitializeComponent();
        }

        #endregion

        #region Override Region

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadStreamList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion

        #region Private Method

        private void LoadStreamList()
        {
            #if RELEASE

            int counter = 0;
            string stringTemp;

            while(Preferences.ContainsKey(Utility.Utility.Key + Convert.ToString(counter)))
            {
                stringTemp = Preferences.Get(Utility.Utility.Key + Convert.ToString(counter), string.Empty);
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
            
            #endif

            streamList.ItemsSource = StreamUrl;
        }

        #endregion

        #region Event Handler

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            string itemElements = (sender as MenuItem).CommandParameter.ToString();
            int counter = StreamUrl.IndexOf(itemElements);

            #if RELEASE

            Preferences.Set(Utility.Utility.Key + Convert.ToString(counter), string.Empty);

            do
            {
                Preferences.Set(Utility.Utility.Key + Convert.ToString(counter), Preferences.Get(Utility.Utility.Key + Convert.ToString(counter + 1), string.Empty));
                counter++;
            }
            while (Preferences.ContainsKey(Utility.Utility.Key + Convert.ToString(counter + 1)));

            Preferences.Remove(Utility.Utility.Key + Convert.ToString(counter));
            
            #endif
            StreamUrl.Remove((sender as MenuItem).CommandParameter.ToString());
        }

        //private void AddButton_Clicked(object sender, EventArgs e)
        //{
        //    #if RELEASE

        //    if (!string.IsNullOrEmpty(entryUrl.Text))
        //        Preferences.Set(Utility.Utility.Key + CounterUrl, entryUrl.Text);

        //    #endif

        //    LoadStreamList();
        //}

        #endregion

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
#if DEBUG
            DependencyService.Get<IMessage>().ShortAlert("btnAdd Clicked");
#endif
            await Navigation.PushModalAsync(new AddStreamUrlPage());
        }
    }
}