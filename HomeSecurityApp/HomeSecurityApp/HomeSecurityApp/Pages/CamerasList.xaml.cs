using System;
using System.Collections.Generic;

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
    public partial class CamerasList : ContentPage
    {
        #region Variables

        public ObservableCollection<CameraObject> CameraObjectList { get; set; } = new ObservableCollection<CameraObject>();

        #endregion

        #region Constructors

        public CamerasList()
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
                LoadCameraObjectList();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"CamerasList - OnAppearing: {ex.Message}");
#if DEBUG
                Trace.WriteLine($"CamerasList - OnAppearing: {ex.Message}");
#endif
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion

        #region Private Method

        private void LoadCameraObjectList()
        {
            CameraObjectList.Clear();

            List<string> PreferencesList = GetPreferencesList();

            foreach (string preference in PreferencesList)
            {
                CameraObjectList.Add(new CameraObject(preference));
            }

            if (CameraObjectList.Count > 0)
                cvCamerasList.ItemsSource = CameraObjectList;
        }

        #endregion

        #region Event Handler

        private async void cvCamerasList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count > 0)
                {
                    DependencyService.Resolve<IMessage>().ShortAlert((e.CurrentSelection[0] as CameraObject).Name);
                    cvCamerasList.SelectedItem = null;

                    SingleCameraVisualization singleCameraVisualization = new SingleCameraVisualization((e.CurrentSelection[0] as CameraObject));
                    singleCameraVisualization.Appearing += SingleCameraVisualizationModal_Appearing;
                    singleCameraVisualization.Disappearing += SingleCameraVisualizationModal_Disappearing;

                    await Navigation.PushModalAsync(singleCameraVisualization);
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"CamerasList - LvCamerasList_ItemTapped: {ex.Message}");
#if DEBUG
                Trace.TraceError($"CamerasList - LvCamerasList_ItemTapped: {ex.Message}");
#endif
            }
        }

        private void SingleCameraVisualizationModal_Appearing(object sender, EventArgs e)
        {
            try
            {
                DeviceDisplay.KeepScreenOn = true;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"CamerasList - SingleStreamVisualizationModal_Appearing: {ex.Message}");
#if DEBUG
                Trace.TraceError($"CamerasList - SingleStreamVisualizationModal_Appearing: {ex.Message}");
#endif
            }
        }

        private void SingleCameraVisualizationModal_Disappearing(object sender, EventArgs e)
        {
            try
            {
                DeviceDisplay.KeepScreenOn = false;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"CamerasList - SingleStreamVisualizationModal_Disappearing: {ex.Message}");
#if DEBUG
                Trace.TraceError($"CamerasList - SingleStreamVisualizationModal_Disappearing: {ex.Message}");
#endif
            }
        }

        #endregion
    }
}