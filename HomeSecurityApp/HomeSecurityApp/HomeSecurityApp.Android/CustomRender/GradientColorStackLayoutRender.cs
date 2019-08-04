using Android.Content;
using HomeSecurityApp.Droid.CustomRender;
using HomeSecurityApp.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.FastRenderers;

[assembly: ExportRenderer(typeof(GradientColorStackLayout), typeof(GradientColorStackLayoutRender))]
namespace HomeSecurityApp.Droid.CustomRender
{
    public class GradientColorStackLayoutRender : VisualElementRenderer<StackLayout>
    {
        private Color StartColor { get; set; }

        private Color CenterColor { get; set; }

        private Color EndColor { get; set; }

        private float YCenter { get; set; }

        private ItemsLayoutOrientation Direction { get; set; }

        private bool HasCenterColor { get; set; }

        public GradientColorStackLayoutRender(Context context) : base(context) { }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            Android.Graphics.LinearGradient LinearGradient = null;

            if(Direction == ItemsLayoutOrientation.Vertical)
            {
                if (HasCenterColor)
                {
                    int[] colors = { this.StartColor.ToAndroid().ToArgb(), this.CenterColor.ToAndroid().ToArgb(), this.EndColor.ToAndroid().ToArgb() };
                    float[] positions = { 0.5F, YCenter, 1 };
                    LinearGradient = new Android.Graphics.LinearGradient(0, 0, 0, Height, colors, positions, Android.Graphics.Shader.TileMode.Mirror);
                }
                else
                {
                    int[] colors = { this.StartColor.ToAndroid().ToArgb(), this.EndColor.ToAndroid().ToArgb() };
                    float[] positions = { YCenter, 1 };
                    LinearGradient = new Android.Graphics.LinearGradient(0, 0, 0, Height, colors, positions, Android.Graphics.Shader.TileMode.Mirror);
                }
            }

            if (Direction == ItemsLayoutOrientation.Horizontal)
            {
                if (HasCenterColor)
                {
                    int[] colors = { this.StartColor.ToAndroid().ToArgb(), this.CenterColor.ToAndroid().ToArgb(), this.EndColor.ToAndroid().ToArgb() };
                    float[] positions = { 0.5F, YCenter, 1 };
                    LinearGradient = new Android.Graphics.LinearGradient(0, 0, 0, Width, colors, positions, Android.Graphics.Shader.TileMode.Mirror);
                }
                else
                {
                    int[] colors = { this.StartColor.ToAndroid().ToArgb(), this.EndColor.ToAndroid().ToArgb() };
                    float[] positions = { YCenter, 1 };
                    LinearGradient = new Android.Graphics.LinearGradient(0, 0, 0, Width, colors, positions, Android.Graphics.Shader.TileMode.Mirror);
                }
            }
            
            Android.Graphics.Paint paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(LinearGradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var stack = e.NewElement as GradientColorStackLayout;
                StartColor = stack.StartColor;
                CenterColor = stack.CenterColor;
                EndColor = stack.EndColor;
                YCenter = stack.YCenter;
                Direction = stack.Direction;
                HasCenterColor = stack.HasCenterColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
}