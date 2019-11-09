using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HomeSecurityApp.Utility
{
    public class GradientColorStackLayout : StackLayout
    {
        public static BindableProperty StartColorProperty =
            BindableProperty.Create(
                nameof(StartColor),
                typeof(Color),
                typeof(GradientColorStackLayout));
        public Color StartColor { get => (Color)GetValue(StartColorProperty); set => SetValue(StartColorProperty, value); }

        public static BindableProperty CenterColorProperty =
            BindableProperty.Create(
                nameof(CenterColor),
                typeof(Color),
                typeof(GradientColorStackLayout));

        public Color CenterColor
        {
            get => (Color)GetValue(CenterColorProperty);
            set
            {
                HasCenterColor = true;
                SetValue(CenterColorProperty, value);
            }
        }

        public static BindableProperty EndColorProperty =
            BindableProperty.Create(
                nameof(EndColor),
                typeof(Color),
                typeof(GradientColorStackLayout));

        public Color EndColor { get => (Color)GetValue(EndColorProperty); set => SetValue(EndColorProperty, value); }

        //TODO: BindableProperty
        public float YStart { get; set; }

        public static BindableProperty YCenterProperty =
            BindableProperty.Create(
                nameof(YCenter),
                typeof(float),
                typeof(GradientColorStackLayout));

        public float YCenter { get => (float)GetValue(YCenterProperty); set => SetValue(YCenterProperty, value); }

        //TODO: BindableProperty
        public float YEnd { get; set; }

        public static BindableProperty DirectionProperty =
            BindableProperty.Create(
                nameof(Direction),
                typeof(ItemsLayoutOrientation),
                typeof(GradientColorStackLayout));
        
        public ItemsLayoutOrientation Direction { get => (ItemsLayoutOrientation)GetValue(DirectionProperty); set => SetValue(DirectionProperty, value); }

        public bool HasCenterColor { get; set; }
    }
}
