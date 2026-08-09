using System.ComponentModel;

using Chapter6.Models;

namespace Chapter6.ViewModels;

public partial class UserViewModel : INotifyPropertyChanged
{
    public string? Username
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(Username));
            }
        }
    }

    public string? Password
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}