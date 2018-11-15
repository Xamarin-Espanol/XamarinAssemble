using Android.Speech.Tts;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinAssemble.Droid;

// uncomment the below line when TextToSpeech is implemented
[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplementation))]
namespace XamarinAssemble.Droid
{
    // Add TextToSpeecImmplementation class here
    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text)
        {
            toSpeak = text;
         
            if (speaker == null)
            {
                speaker = new TextToSpeech(MainActivity.Instance, this);
            }
            else
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
        #endregion
    }
}