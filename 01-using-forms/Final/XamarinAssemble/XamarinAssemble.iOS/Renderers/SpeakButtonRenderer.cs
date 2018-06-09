using MyEvents.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinAssemble;

[assembly: ExportRenderer(typeof(SpeakButton), typeof(SpeakButtonRenderer))]
namespace MyEvents.iOS.Renderers
{
    public class SpeakButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.BorderWidth = 0.5f;
                Control.Layer.BorderColor = UIColor.Gray.CGColor;
                Control.Layer.CornerRadius = 8f;
            }
        }
    }
}