using HomeSecurityApp.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurityApp.ViewModels
{
    public class CameraObjectViewModel : BaseViewModel
    {
        #region Variables

        public string Key { get; set; }

        public string Name { get; set; }

        public string ConnectionUrl { get; set; }

        public bool Selected { get; set; }

        public string Icon { get { return Selected ? FontAwesomeGlyph.Chevron_Down : FontAwesomeGlyph.Chevron_Up; } }

        #endregion

        #region Constructor

        public CameraObjectViewModel() { }

        public CameraObjectViewModel(string Key)
        {
            if (string.IsNullOrEmpty(Key))
                return;

            string[] keySplit = Key.Split('#');

            this.Key = Key;
            this.Name = keySplit[0];
            this.ConnectionUrl = keySplit[1];
        }

        #endregion

        #region Event Handler

        #endregion

        #region Private Method

        #endregion
    }
}
