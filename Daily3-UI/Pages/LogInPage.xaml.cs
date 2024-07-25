using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

/// <summary>
///     Page for handling logging into the application
/// </summary>
public partial class LogInPage : ContentPage
{
    public LogInPage()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Logging you in if you have a account
    /// </summary>
    private async void AttemptLogIn(object sender, EventArgs e)
    {
        ErrorLabel.Text = "";
        var request_response = await VerifyUserClient.VerifyUser(Username.Text, Password.Text);
        try
        {
            Globals.UserId = Guid.Parse(request_response);
            Application.Current.MainPage = new AppShell();
        }
        catch
        {
            ErrorLabel.Text = request_response;
        }
    }

    /// <summary>
    ///     Takes you to the create account in page.
    /// </summary>
    private void TakeToCreateAccountPage(object sender, EventArgs e)
    {
        Application.Current.MainPage = new CreateAccountPage();
    }
}