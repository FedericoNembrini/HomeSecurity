using HomeSecurityApp.Utility;
using HomeSecurityApp.Utility.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public ObservableCollection<StreamObject> StreamObjectList { get; set; } = new ObservableCollection<StreamObject>();

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

            try
            {
                LoadStreamListObject();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamListManagement - OnAppearing: {ex.Message}");
                Trace.TraceError($"StreamListManagement - OnAppearing: {ex.Message}");
            }
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
                StreamObjectList.Add(new StreamObject(preference, false));
            }

            if(StreamObjectList.Count > 0)
                streamList.ItemsSource = StreamObjectList;
        }

        #endregion

        #region Event Handler

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            AddStreamUrlPage addStreamUrlPageModal = new AddStreamUrlPage(StreamObjectList.Count);
            addStreamUrlPageModal.Disappearing += AddStreamUrlPageModal_Disappearing;
            await Navigation.PushModalAsync(addStreamUrlPageModal);
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string itemElements = (sender as MenuItem).CommandParameter.ToString();
                var streamObject = StreamObjectList.Where(sol => sol.Key == itemElements).FirstOrDefault();
                if (streamObject != null)
                {
                    var counter = StreamObjectList.IndexOf(streamObject);

                    Preferences.Set(Key + Convert.ToString(counter), string.Empty);
                    do
                    {
                        Preferences.Set(Key + Convert.ToString(counter), Preferences.Get(Key + Convert.ToString(counter + 1), string.Empty));
                        counter++;
                    }
                    while (Preferences.ContainsKey(Key + Convert.ToString(counter + 1)));

                    Preferences.Remove(Key + Convert.ToString(counter));

                    StreamObjectList.Remove(streamObject);
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamListManagement - DeleteButton_Clicked: {ex.Message}");
                Trace.TraceError($"StreamListManagement - DeleteButton_Clicked: {ex.Message}");
            }
        }

        private void AddStreamUrlPageModal_Disappearing(object sender, EventArgs e)
        {
            try
            {
                LoadStreamListObject();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamListManagement - AddStreamUrlPageModal_Disappearing: {ex.Message}");
                Trace.TraceError($"StreamListManagement - AddStreamUrlPageModal_Disappearing: {ex.Message}");
            }
        }

        #endregion
    }
}