using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HomeSecurityApp.Droid.CustomRender;
using HomeSecurityApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShellApp), typeof(GradientShellRender))]
namespace HomeSecurityApp.Droid.CustomRender
{
    public class GradientShellRender : ShellRenderer
    {
        public GradientShellRender(Context context) : base(context) { }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new ShellToolbarOverride(this);
        }
    }

   
}