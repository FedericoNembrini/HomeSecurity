using System;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Diagnostics;
using HomeSecurityApp.Utility.Interface;
using HomeSecurityApp.ViewModels;

namespace HomeSecurityApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CamerasList : ContentPage
    {
        #region Variables

        public CamerasListViewModel ViewModel { get; set; }

        #endregion

        #region Constructors

        public CamerasList()
        {
            InitializeComponent();

            BindingContext = ViewModel = new CamerasListViewModel();
        }

        #endregion

        #region Override Method

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.UpdateCameraObjectList();
        }

        #endregion

        #region Private Method


        #endregion

        #region Event Handler

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