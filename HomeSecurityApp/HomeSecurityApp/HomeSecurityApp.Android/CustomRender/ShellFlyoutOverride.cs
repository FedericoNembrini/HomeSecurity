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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace HomeSecurityApp.Droid.CustomRender
{
    public class ShellFlyoutOverride : ShellFlyoutRenderer
    {
        IShellContext ShellContext;

        public ShellFlyoutOverride(IShellContext shellContext, Context context) : base(shellContext, context)
        {
            ShellContext = shellContext;
        }

        protected override void AttachFlyout(IShellContext context, Android.Views.View content)
        {
            base.AttachFlyout(context, content);
        }
    }
}