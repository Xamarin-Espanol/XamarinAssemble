using System;
using Xamarin.Forms;
using XamarinAssemble.Models;
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

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Session;
            if (item == null)
                return;

            await Navigation.PushAsync(new SessionDetailPage() { BindingContext = new SessionDetailViewModel(item) });

            // Manually deselect item
            SessionsListView.SelectedItem = null;
        }
    }
}
