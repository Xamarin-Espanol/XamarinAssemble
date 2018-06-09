using Xamarin.Forms;
using XamarinAssemble.ViewModels;

namespace XamarinAssemble.Views
{
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel aboutViewModel;

        public AboutPage()
        {
            InitializeComponent();

            aboutViewModel = new AboutViewModel();
            this.BindingContext = aboutViewModel;
        }
    }
}
