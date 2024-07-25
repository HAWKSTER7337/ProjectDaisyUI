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
        var clientResponse = await CreateAccountClient.CreateAccount(Username.Text, Password.Text);
        var successString = "User has been added to the database now you have to wait for approval";
        ErrorLabel.TextColor = clientResponse == successString
            ? BuyTickets.GetColor("SuccessGreen")
            : BuyTickets.GetColor("DailyRed");
        ErrorLabel.Text = clientResponse;
    }

    private void ToLogin(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new LogInPage();
    }
}