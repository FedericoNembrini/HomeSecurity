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
                LoadStreamObjectList();

                lvStreamList.ItemsSource = StreamObjectList;
            }
            catch (Exception ex)
            {
                #if DEBUG
                    DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    Trace.WriteLine(ex.Message);
                #endif
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

        private void LoadStreamObjectList()
        {
            _LibVlc = new LibVLC();

            StreamObjectList.Clear();

            List<string> PreferencesList = GetPreferencesList();
            
            foreach(string preference in PreferencesList)
            {
                StreamObjectList.Add(new StreamListObject(preference, true, _LibVlc));
            }   
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
                Trace.TraceError(ex.Message);
                #endif
            }
        }
    }
}