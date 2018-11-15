using System.Windows.Input;
using Xamarin.Forms;
using XamarinAssemble.Models;

namespace XamarinAssemble.ViewModels
{
    public class SessionDetailViewModel : ViewModelBase
    {
        private string formattedTime = string.Empty;

        public string SessionName { get { return SelectedSession?.Title; } }

        public string SpeakerName { get { return SelectedSession?.Presenter; } }

        public string Abstract { get { return SelectedSession?.Abstract; } }

        public string Time { get { return formattedTime; } }

        private Sessions selectedSession;

        public Sessions SelectedSession
        {
            get { return selectedSession; }
            set { if (selectedSession == value) return; selectedSession = value; OnPropertyChanged(nameof(SelectedSession)); }
        }

        public ICommand SpeakCommand { get; set; }

        public SessionDetailViewModel(Sessions selectedSession = null)
        {
            SelectedSession = selectedSession;

            var start = SelectedSession?.Start;
            var startString = start?.ToLocalTime().ToString("t");
            var end = SelectedSession?.End;
            var endString = end?.ToLocalTime().ToString("t");
            var day = start?.ToString("M");

            formattedTime = $"{day}, {startString}–{endString}";

            SpeakCommand = new Command(() =>
            {
                // add DependencyService call here
                DependencyService.Get<ITextToSpeech>().Speak($"Session {SessionName} presented by {SpeakerName} is on {Time}");
            });
        }
    }
}
