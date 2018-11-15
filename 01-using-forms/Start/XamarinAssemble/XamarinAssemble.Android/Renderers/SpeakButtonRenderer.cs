using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinAssemble;
using XamarinAssemble.Droid.Renderers;

[assembly: ExportRenderer(typeof(SpeakButton), typeof(SpeakButtonRenderer))]
namespace XamarinAssemble.Droid.Renderers
{
    public class SpeakButtonRenderer : ButtonRenderer
    {
        // Add Android specific customization here
    }
}