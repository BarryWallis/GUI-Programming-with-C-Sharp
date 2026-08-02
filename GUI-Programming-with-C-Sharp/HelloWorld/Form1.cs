namespace HelloWorld;

public partial class Form1 : Form
{
    public Form1() => InitializeComponent();

    private void BtnGreet_Click(object sender, EventArgs e)
        => MessageBox.Show($"Hello {(string.IsNullOrWhiteSpace(txtName.Text) ? "World" : txtName.Text)}");
}
