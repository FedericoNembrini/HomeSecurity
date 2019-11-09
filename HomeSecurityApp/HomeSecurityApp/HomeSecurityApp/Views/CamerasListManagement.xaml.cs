using HomeSecurityApp.Controls;
using HomeSecurityApp.Utility.Interface;
using HomeSecurityApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static HomeSecurityApp.Utility.Utility;

namespace HomeSecurityApp.Pages
{
    [DesignTimeVisible(false)]
    //[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CamerasListManagement : ContentPage
	{
        #region Variables

        CamerasListManagementViewModel camerasListManagementViewModel = new CamerasListManagementViewModel();

        uint animationSpeed = 300;
        
        CameraInformationCell selectedElement;
        
        int selectedIndex;

        #endregion

        #region Constructor

        public CamerasListManagement()
        {
            InitializeComponent();

            BindingContext = camerasListManagementViewModel;
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
#if DEBUG
                Trace.TraceError($"StreamListManagement - OnAppearing: {ex.Message}");
#endif
            }
        }

        protected override void OnDisappearing()
        {
            cvCamerasList.SelectedItems.Clear();
            cvCamerasList.SelectedItem = null;

            base.OnDisappearing();
        }

        #endregion

        #region Private Method

        private void LoadStreamListObject()
        {
            camerasListManagementViewModel.CameraObjectList.Clear();

            List<string> PreferencesList = GetPreferencesList();

            foreach(string preference in PreferencesList)
            {
                camerasListManagementViewModel.CameraObjectList.Add(new CameraObjectViewModel(preference));
            }
        }

        private void PositionElement(VisualElement parent, VisualElement element)
        {
            AbsoluteLayout.SetLayoutFlags(element, AbsoluteLayoutFlags.None);
            var dropDownContainerRect = new Rectangle(0, parent.Bounds.Top, this.Width, element.Height);
            AbsoluteLayout.SetLayoutBounds(element, dropDownContainerRect);
        }

        private async Task DisplayCommand(View view)
        {
            view.IsVisible = true;
            view.RotationX = -90;
            view.Opacity = 0;
            _ = view.FadeTo(1, animationSpeed);
            await view.RotateXTo(0, animationSpeed);
        }

        #endregion

        #region Event Handler

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                AddStreamUrlPage addStreamUrlPageModal = new AddStreamUrlPage(camerasListManagementViewModel.CameraObjectList.Count);

                addStreamUrlPageModal.Disappearing += AddStreamUrlPageModal_Disappearing;

                await Navigation.PushModalAsync(addStreamUrlPageModal);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamListManagement - BtnAdd_Clicked: {ex.Message}");
#if DEBUG
                Trace.WriteLine($"StreamListManagement - BtnAdd_Clicked: {ex.Message}");
#endif
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string itemElements = (sender as MenuItem).CommandParameter.ToString();
                var streamObject = camerasListManagementViewModel.CameraObjectList.Where(sol => sol.Key == itemElements).FirstOrDefault();
                if (streamObject != null)
                {
                    var counter = camerasListManagementViewModel.CameraObjectList.IndexOf(streamObject);

                    Preferences.Set(Key + Convert.ToString(counter), string.Empty);
                    do
                    {
                        Preferences.Set(Key + Convert.ToString(counter), Preferences.Get(Key + Convert.ToString(counter + 1), string.Empty));
                        counter++;
                    }
                    while (Preferences.ContainsKey(Key + Convert.ToString(counter + 1)));

                    Preferences.Remove(Key + Convert.ToString(counter));

                    camerasListManagementViewModel.CameraObjectList.Remove(streamObject);
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert($"StreamListManagement - DeleteButton_Clicked: {ex.Message}");
#if DEBUG
                Trace.TraceError($"StreamListManagement - DeleteButton_Clicked: {ex.Message}");
#endif
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
#if DEBUG
                Trace.TraceError($"StreamListManagement - AddStreamUrlPageModal_Disappearing: {ex.Message}");
#endif
            }
        }

        #endregion

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            selectedElement = sender as CameraInformationCell;
            selectedIndex = camerasListManagementViewModel.CameraObjectList.IndexOf(selectedElement.BindingContext as CameraObjectViewModel);

            (selectedElement.BindingContext as CameraObjectViewModel).Selected = true;

            //FakeCameraInformationCell.BindingContext = selectedElement.BindingContext;

            //PositionElement(selectedElement, gFrontSide);

            // Fade in the overlay
            slFadeBackground.Opacity = 0;
            slFadeBackground.IsVisible = true;
            _ = slFadeBackground.FadeTo(1, animationSpeed);

            //await DisplayCommand(Delete);
        }

        private void DeleteButton_Tapped(object sender, EventArgs e)
        {

        }

        private async void CancelModify_Clicked(object sender, EventArgs e)
        {
            _ = await slFadeBackground.FadeTo(0, animationSpeed);
            slFadeBackground.IsVisible = false;

            cvCamerasList.SelectedItems.Clear();
            cvCamerasList.SelectedItem = null;
        }
    }
}