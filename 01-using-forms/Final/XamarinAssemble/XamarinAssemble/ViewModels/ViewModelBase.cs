using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace XamarinAssemble.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { if (isBusy == value) return; isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { if (title == value) return; title = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        public ICommand RefreshCommand { get; set; }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
