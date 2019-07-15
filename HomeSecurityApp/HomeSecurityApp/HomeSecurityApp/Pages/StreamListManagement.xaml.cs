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
using static HomeSecurityApp.Utility.Utility;

namespace HomeSecurityApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StreamListManagement : ContentPage
	{
        #region Variables

        public ObservableCollection<StreamListObject> StreamObjectList { get; set; } = new ObservableCollection<StreamListObject>();

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

            LoadStreamListObject();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion

        #region Private Method

        private void LoadStreamListObject()
        {
            StreamObjectList.Clear();

            List<string> PreferencesList = GetPreferencesList();

            foreach(string preference in PreferencesList)
            {
                StreamObjectList.Add(new StreamListObject(preference, false));
            }

            streamList.ItemsSource = StreamObjectList;
        }

        #endregion

        #region Event Handler

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            #if DEBUG
                DependencyService.Get<IMessage>().ShortAlert("btnAdd Clicked");
            #endif

            await Navigation.PushModalAsync(new AddStreamUrlPage(StreamObjectList.Count));
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            string itemElements = (sender as MenuItem).CommandParameter.ToString();
            var streamObject = StreamObjectList.Where(sol => sol.Key == itemElements).FirstOrDefault();
            var index = StreamObjectList.IndexOf(streamObject);
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
            StreamObjectList.Remove(streamObject);
        }

        #endregion
    }
}