using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LibVLCSharp.Shared;
using System.Diagnostics;
using HomeSecurityApp.Utility;
using static HomeSecurityApp.Utility.Utility;
using System.Collections.ObjectModel;
using HomeSecurityApp.Utility.Interface;

namespace HomeSecurityApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StreamList : ContentPage
    {
        #region Variables

        LibVLC _LibVlc;

        public ObservableCollection<StreamObject> StreamObjectList { get; set; } = new ObservableCollection<StreamObject>();

        #endregion

        #region Constructors

        public StreamList()
        {
            InitializeComponent();
            Core.Initialize();
            _LibVlc = new LibVLC();
        }

        #endregion

        #region Oerride Method

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                LoadStreamObjectList();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamList - OnAppearing: {ex.Message}");
                Trace.WriteLine($"StreamList - OnAppearing: {ex.Message}");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //try
            //{
                
            //}
            //catch (Exception ex)
            //{
            //    DependencyService.Get<IMessage>().LongAlert($"StreamList - OnDisappearing: {ex.Message}");
            //    Trace.WriteLine(ex.Message);
            //}
        }

        #endregion

        #region Private Method

        private void LoadStreamObjectList()
        {

            StreamObjectList.Clear();

            List<string> PreferencesList = GetPreferencesList();

            foreach (string preference in PreferencesList)
            {
                StreamObjectList.Add(new StreamObject(preference, true, _LibVlc));
            }

            if (StreamObjectList.Count > 0)
                lvStreamList.ItemsSource = StreamObjectList;
        }

        #endregion

        #region Event Handler

        private async void LvStreamList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                SingleStreamVisualization singleStreamVisualizationModal = new SingleStreamVisualization((e.Item as StreamObject).MediaPlayer);
                singleStreamVisualizationModal.Appearing += SingleStreamVisualizationModal_Appearing;
                singleStreamVisualizationModal.Disappearing += SingleStreamVisualizationModal_Disappearing;

                await Navigation.PushModalAsync(singleStreamVisualizationModal);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamList - LvStreamList_ItemTapped: {ex.Message}");
                Trace.TraceError($"StreamList - LvStreamList_ItemTapped: {ex.Message}");
            }
        }

        private void SingleStreamVisualizationModal_Appearing(object sender, EventArgs e)
        {
            DeviceDisplay.KeepScreenOn = true;
        }

        private void SingleStreamVisualizationModal_Disappearing(object sender, EventArgs e)
        {
            DeviceDisplay.KeepScreenOn = false;
        }

        #endregion
    }
}