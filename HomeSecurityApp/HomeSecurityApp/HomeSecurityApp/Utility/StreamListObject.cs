using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace HomeSecurityApp.Utility
{
    public class StreamListObject
    {
        #region Variables

        public string Name { get; set; }

        public string ConnectionUrl { get; set; }

        public bool Status { get; set; }

        public MediaPlayer MediaPlayer { get; set; }

        #endregion

        #region Constructor

        public StreamListObject(string Key, bool LoadMediaPlayer, LibVLC LibVLCInstance = null)
        {
            if (string.IsNullOrEmpty(Key))
                return;

            string[] keySplit = Key.Split('#');

            Name = keySplit[0];
            ConnectionUrl = keySplit[1];

            if (LoadMediaPlayer)
            {            
                MediaPlayer = new MediaPlayer(LibVLCInstance)
                {
                    Media = new Media(LibVLCInstance,
                        ConnectionUrl,
                        FromType.FromLocation)
                };

                MediaPlayer.Volume = 0;

                MediaPlayer.Play();
                Status = MediaPlayer.IsPlaying;
                MediaPlayer.Stop();
            }
        }

        #endregion

        #region Event Handler

        #endregion
    }
}
