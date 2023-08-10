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
    /// Logging you in if you have a account
    /// </summary>
    private async void AttemptLogIn(object sender, EventArgs e)
    {
        var userID = await VerifyUserClient.VerifyUser(Username.Text, Password.Text);

        if (userID is not null) Application.Current.MainPage = new AppShell();
    }
}