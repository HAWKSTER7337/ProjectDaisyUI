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
        Globals.UserId = await VerifyUserClient.VerifyUser(Username.Text, Password.Text);
        if (Globals.UserId is null)
        {
            ErrorLabel.Text = "Incorrect UserName Or Password";
            return;
        }

        Application.Current.MainPage = new AppShell();
    }

    /// <summary>
    ///     Takes you to the create account in page.
    /// </summary>
    private void TakeToCreateAccountPage(object sender, EventArgs e)
    {
        Application.Current.MainPage = new CreateAccountPage();
    }
}