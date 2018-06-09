using System.Runtime.CompilerServices;
using Xamarin.Forms;
using XamarinAssemble.ViewModels;

namespace XamarinAssemble.Views
{
    public partial class SessionDetailPage : ContentPage
    {
        public SessionDetailViewModel ViewModel
        {
            get { return (SessionDetailViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create(nameof(ViewModel), typeof(SessionDetailViewModel), typeof(SessionDetailPage), false);

        public SessionDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ViewModel))
            {
                if (ViewModel != null)
                {
                    BindingContext = ViewModel;
                }
            }
        }
    }
}
