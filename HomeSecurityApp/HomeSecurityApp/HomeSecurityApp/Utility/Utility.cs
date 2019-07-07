using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace HomeSecurityApp.Utility
{
    public static class Utility
    {
        public const string Key = "StreamUrl_";

        public static List<string> GetPreferencesList()
        {
            int counter = 0;
            string preferenceValue;
            List<string> preferencesValueList = new List<string>();

#if Release
            while (Preferences.ContainsKey(Key + Convert.ToString(counter)))
            {
                preferenceValue = Preferences.Get(Key + Convert.ToString(counter), string.Empty);
                if (!string.IsNullOrEmpty(preferenceValue))
                {
                    preferencesValueList.Add(preferenceValue);
                }
                counter++;
            }
#elif DEBUG
            preferencesValueList.Add("Test#rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov");
#endif
            return preferencesValueList;
        }
    }
}
