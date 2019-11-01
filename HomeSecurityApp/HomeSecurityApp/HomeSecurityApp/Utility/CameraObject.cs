using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurityApp.Utility
{
    public class CameraObject
    {
        #region Variables

        public string Key { get; set; }

        public string Name { get; set; }

        public string ConnectionUrl { get; set; }

        public bool Status { get; set; } = true;

        #endregion

        #region Constructor

        public CameraObject(string Key)
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
