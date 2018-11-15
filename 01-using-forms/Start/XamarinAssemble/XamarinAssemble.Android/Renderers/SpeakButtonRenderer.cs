using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinAssemble;
using XamarinAssemble.Droid.Renderers;

[assembly: ExportRenderer(typeof(SpeakButton), typeof(SpeakButtonRenderer))]
namespace XamarinAssemble.Droid.Renderers
{
    public class SpeakButtonRenderer : ButtonRenderer
    {
        public SpeakButtonRenderer(Context context) : base(context)
        {
        }

        // Add Android specific customization here
    }
}