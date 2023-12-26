using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Months18.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy;
        string? title;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !isBusy;
        public string Title
        {
            get => title ?? string.Empty;
            set
            {
                if (title == value)
                    return;
                title = value;

                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)=>
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
    }
}