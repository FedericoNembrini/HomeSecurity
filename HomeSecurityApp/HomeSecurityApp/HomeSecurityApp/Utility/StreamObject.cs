using HomeSecurityApp.Utility.Interface;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeSecurityApp.Utility
{
    public class StreamObject
    {
        #region Variables

        public string Key { get; set; }

        public string Name { get; set; }

        public string ConnectionUrl { get; set; }

        public bool Status { get; set; } = true;

        public MediaPlayer MediaPlayer { get; set; }

        #endregion

        #region Constructor

        public StreamObject(string Key, bool LoadMediaPlayer, LibVLC LibVLCInstance = null)
        {
            if (string.IsNullOrEmpty(Key))
                return;

            string[] keySplit = Key.Split('#');

            this.Key = Key;
            this.Name = keySplit[0];
            this.ConnectionUrl = keySplit[1];

            if (LoadMediaPlayer)
            {            
                this.MediaPlayer = new MediaPlayer(LibVLCInstance)
                {
                    Media = new Media(LibVLCInstance,
                        ConnectionUrl,
                        FromType.FromLocation)
                };

                this.MediaPlayer.Volume = 0;
            }
        }

        #endregion

        #region Event Handler

        #endregion

        #region Private Method

        #endregion
    }
}
