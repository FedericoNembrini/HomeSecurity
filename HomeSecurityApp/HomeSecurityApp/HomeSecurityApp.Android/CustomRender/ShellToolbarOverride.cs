using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace HomeSecurityApp.Droid.CustomRender
{
    public class ShellToolbarOverride : ShellToolbarAppearanceTracker
    {
        IShellContext Context;

        public ShellToolbarOverride(IShellContext context) : base(context)
        {
            Context = context;
        }

        public override void SetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            base.SetAppearance(toolbar, toolbarTracker, appearance);

            toolbar.SetBackgroundColor(new Android.Graphics.Color(Context.AndroidContext.GetColor(Resource.Color.BackgroundGradientStart)));
            toolbar.Elevation = 0F;
            toolbar.TranslationZ = 0F;
        }
    }
}