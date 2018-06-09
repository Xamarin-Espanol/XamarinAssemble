using System;
using Xamarin.Forms;
using XamarinAssemble.ViewModels;

namespace XamarinAssemble.Views
{
    public partial class SessionsPage : ContentPage
    {
        private SessionsViewModel sessionsViewModel;
        public SessionsPage()
        {
            InitializeComponent();

            sessionsViewModel = new SessionsViewModel();
            BindingContext = sessionsViewModel;
        }

        // Add OnItemSelected event handler here
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await sessionsViewModel.Initialization;
        }
    }
}
