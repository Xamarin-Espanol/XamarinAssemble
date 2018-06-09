using Xamarin.Forms;
using XamarinAssemble.ViewModels;

namespace XamarinAssemble.Views
{
    public partial class SpeakersPage : ContentPage
    {
        private SpeakersViewModel speakersViewModel;

        public SpeakersPage()
        {
            InitializeComponent();

            speakersViewModel = new SpeakersViewModel();
            BindingContext = speakersViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await speakersViewModel.Initialization;
        }
    }
}
