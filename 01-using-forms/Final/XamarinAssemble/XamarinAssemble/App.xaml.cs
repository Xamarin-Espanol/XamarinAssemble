using XamarinAssemble.Views;

using Xamarin.Forms;

namespace XamarinAssemble
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var mainPage = new TabbedPage();
            var sessionsPage = new NavigationPage(new SessionsPage()) { Title = "Sessions" };
            var speakersPage = new NavigationPage(new SpeakersPage()) { Title = "Speakers" };
            var aboutPage = new NavigationPage(new AboutPage()) { Title = "About" };

            mainPage.Children.Add(sessionsPage);
            mainPage.Children.Add(speakersPage);
            mainPage.Children.Add(aboutPage);

            if (Device.RuntimePlatform == Device.iOS)
            {
                sessionsPage.Icon = "tab_feed.png";
                speakersPage.Icon = "tab_person.png";
                aboutPage.Icon = "tab_about.png";
            }

            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
