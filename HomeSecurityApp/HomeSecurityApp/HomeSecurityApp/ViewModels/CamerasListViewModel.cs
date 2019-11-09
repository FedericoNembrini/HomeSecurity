using HomeSecurityApp.Pages;
using HomeSecurityApp.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeSecurityApp.ViewModels
{
    public class CamerasListViewModel : BaseViewModel
    {
        #region Variables

        public ObservableCollection<CameraObjectViewModel> CameraObjectList { get; } = new ObservableCollection<CameraObjectViewModel>();

        private ICommand _selectionChangedCommand;

        public ICommand SelectionChangedCommand
        {
            private set => _selectionChangedCommand = value;
            get => _selectionChangedCommand ?? (_selectionChangedCommand = new Command(execute: (object parameter) =>
                {
                    var sender = parameter as CollectionView;

                    if (sender.SelectedItem != null)
                    {
                        SingleCameraVisualization singleCameraVisualization = new SingleCameraVisualization((sender.SelectedItem as CameraObjectViewModel));

                        Navigation.PushModalAsync(singleCameraVisualization);

                        sender.SelectedItem = null;
                    }
                }));
        }

        #endregion

        #region Constructor

        public CamerasListViewModel()
        {
            UpdateCameraObjectList();
        }

        #endregion

        #region Method

        public void UpdateCameraObjectList()
        {
            CameraObjectList.Clear();

            List<string> PreferencesList = Utility.Utility.GetPreferencesList();

            foreach (string preference in PreferencesList)
            {
                CameraObjectList.Add(new CameraObjectViewModel(preference));
            }
        }

        #endregion
    }
}
