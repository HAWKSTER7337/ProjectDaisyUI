using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.Enums;

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
            var guidAndStatusList = SplitResponse(request_response);

            // Adding the users global info
            Globals.UserId = Guid.Parse(guidAndStatusList[0]);
            Globals.Status = (Status?)int.Parse(guidAndStatusList[1]);
            Application.Current.MainPage = new AppShell();
        }
        catch
        {
            var errorCode = e.ToString();
            ErrorLabel.Text = request_response;
        }
    }

    /// <summary>
    ///     Splitting up the response from the back end to make sure
    ///     the value of the GUID of the user as well as the Status are
    ///     both collected
    /// </summary>
    /// <param name="guidAndStatus">The string that is being parsed</param>
    /// <returns>An array of the GUID and Status of the user as a string</returns>
    private string[] SplitResponse(string guidAndStatus)
    {
        char[] separator = { ',' };
        var parts = guidAndStatus.Split(separator);
        return parts;
    }

    /// <summary>
    ///     Takes you to the create account in page.
    /// </summary>
    private void TakeToCreateAccountPage(object sender, EventArgs e)
    {
        Application.Current.MainPage = new CreateAccountPage();
    }

    /// <summary>
    ///     Takes you to the create account in page.
    /// </summary>
    private void TakeToChangePasswordPage(object sender, EventArgs e)
    {
        Application.Current.MainPage = new ChangePasswordPage();
    }
}