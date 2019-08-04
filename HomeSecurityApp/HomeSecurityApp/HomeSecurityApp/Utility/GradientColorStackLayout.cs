using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HomeSecurityApp.Utility
{
    public class GradientColorStackLayout : StackLayout
    {
        public Color StartColor { get; set; }

        private Color _CenterColor;

        public Color CenterColor
        {
            get
            {
                return _CenterColor;
            }
            set
            {
                HasCenterColor = true;
                _CenterColor = value;
            }
        }

        public Color EndColor { get; set; }

        public float YCenter { get; set; } = 0.5F;

        public ItemsLayoutOrientation Direction { get; set; }

        public bool HasCenterColor { get; set; }
    }
}
