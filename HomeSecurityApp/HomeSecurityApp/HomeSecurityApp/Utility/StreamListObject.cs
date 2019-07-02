using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurityApp.Utility
{
    public class StreamListObject
    {
        #region Variables

        string Name;

        string ConnectionUrl;

        bool Status;

        MediaPlayer MediaPlayer { get; set; }

        #endregion

        #region Constructor

        public StreamListObject(string Key, LibVLC LibVLCInstance)
        {
            if (string.IsNullOrEmpty(Key))
                return;

            string[] keySplit = Key.Split('#');

            Name = keySplit[0];
            ConnectionUrl = keySplit[1];

            MediaPlayer = new MediaPlayer(LibVLCInstance)
            {
                Media = new Media(LibVLCInstance,
                    ConnectionUrl,
                    FromType.FromLocation)
            };
        }

        #endregion

    }
}
