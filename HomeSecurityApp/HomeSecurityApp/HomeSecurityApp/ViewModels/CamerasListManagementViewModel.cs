using HomeSecurityApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HomeSecurityApp.ViewModels
{
    public class CamerasListManagementViewModel : BaseViewModel
    {
        public ObservableCollection<CameraObjectViewModel> CameraObjectList { get; set; } = new ObservableCollection<CameraObjectViewModel>();
    }
}
