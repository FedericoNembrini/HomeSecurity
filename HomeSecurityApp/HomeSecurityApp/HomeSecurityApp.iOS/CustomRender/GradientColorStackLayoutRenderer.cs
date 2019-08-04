using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using HomeSecurityApp.iOS.CustomRender;
using HomeSecurityApp.Utility;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientColorStackLayout), typeof(GradientColorStackLayoutRenderer))]
namespace HomeSecurityApp.iOS.CustomRender
{
    public class GradientColorStackLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            GradientColorStackLayout stack = this.Element as GradientColorStackLayout;
            CGColor startColor = stack.StartColor.ToCGColor();
            CGColor centerColor = stack.CenterColor.ToCGColor();
            CGColor endColor = stack.EndColor.ToCGColor();
            float yCenter = stack.YCenter;
            ItemsLayoutOrientation direction = stack.Direction;
            bool hasCenterColor = stack.HasCenterColor;

            CAGradientLayer gradientLayer = null;

            if (direction == ItemsLayoutOrientation.Vertical)
            {
                gradientLayer = new CAGradientLayer();

            }

            if (direction == ItemsLayoutOrientation.Horizontal)
            {
                gradientLayer = new CAGradientLayer()
                {
                    StartPoint = new CGPoint(0, 0.5),
                    EndPoint = new CGPoint(1, 0.5)
                };
            }

            gradientLayer.Frame = rect;

            if (hasCenterColor)
            {
                gradientLayer.Locations = new NSNumber[] { new NSNumber(0.5F), new NSNumber(yCenter), new NSNumber(1F) };
                gradientLayer.Colors = new CGColor[] { startColor, centerColor, endColor };
            }
            else
            {
                gradientLayer.Locations = new NSNumber[] { new NSNumber(yCenter), new NSNumber(1F) };
                gradientLayer.Colors = new CGColor[] { startColor, endColor };
            }

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}