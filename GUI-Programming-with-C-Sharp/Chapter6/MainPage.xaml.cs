using Chapter6.ViewModels;

namespace Chapter6;

public partial class MainPage : ContentPage
{
    public MainPage() => InitializeComponent();

    private async void SubmitButton_Clicked(object? sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;
        await DisplayAlertAsync("Login", $"Username: {username}\nPassword: {password}", "OK");

        if (BindingContext is UserViewModel viewModel)
        {
            viewModel.Username = usernameEntry.Text;
            viewModel.Password = passwordEntry.Text;
        }
    }
}
