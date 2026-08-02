namespace HelloWorldMAUI;

public partial class MainPage : ContentPage
{
    public MainPage() => InitializeComponent();

    private void OnGreetButtonClicked(object sender, EventArgs e) => GreetingLabel.Text = $"Hello {NameEntry.Text}";
}
