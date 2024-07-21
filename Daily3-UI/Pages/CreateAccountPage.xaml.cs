using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class CreateAccountPage : ContentPage
{
    public CreateAccountPage()
    {
        InitializeComponent();
    }

    private async void CreateAccount(object sender, EventArgs e)
    {
        ErrorLabel.Text = "";
        ErrorLabel.Text = await CreateAccountClient.CreateAccount(Username.Text, Password.Text);
    }
}