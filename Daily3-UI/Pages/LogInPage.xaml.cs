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
        ScaleUIForDevice();
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
            await Shell.Current.GoToAsync("Home3");
        }
        catch(Exception ex)
        {
            var errorCode = ex.Message;
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
    private List<string> SplitResponse(string guidAndStatus)
    {
        char[] separator = { ',' };
        var parts = guidAndStatus.Split(separator).ToList();
        return parts;
    }

    /// <summary>
    ///     Takes you to the create account in page.
    /// </summary>
    private async void TakeToCreateAccountPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CreateAccount");
    }

    /// <summary>
    ///     Takes you to the create account in page.
    /// </summary>
    private async void TakeToChangePasswordPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChangePassword");
    }
    
    /// <summary>
    /// Scales the applications buttons and such to look good
    /// on any screen 
    /// </summary>
    private void ScaleUIForDevice()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;

        // Width in pixels / density = device-independent width
        double width = displayInfo.Width / displayInfo.Density;
        double height = displayInfo.Height / displayInfo.Density;

        // Simple scale factor: based on a baseline device width
        double scaleFactor = width / 375.0; // 375 is base width of iPhone 11 for reference

        // Minimum and max limits to prevent extreme scaling
        scaleFactor = Math.Clamp(scaleFactor, 0.8, 1.5);

        // Scale font sizes
        Username.FontSize *= scaleFactor;
        Password.FontSize *= scaleFactor;
        ErrorLabel.FontSize *= scaleFactor;

        // Optionally scale margins/padding (example for button)
        foreach (var child in (this.Content as Layout).Children)
        {
            if (child is Button btn)
            {
                btn.FontSize *= scaleFactor;
                btn.Margin = new Thickness(
                    btn.Margin.Left * scaleFactor,
                    btn.Margin.Top * scaleFactor,
                    btn.Margin.Right * scaleFactor,
                    btn.Margin.Bottom * scaleFactor
                );
            }
            else if (child is Label label)
            {
                label.FontSize *= scaleFactor;
            }
        }
    }
}