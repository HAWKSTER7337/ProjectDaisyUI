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
    Console.WriteLine($"\n\nResponse received: {request_response}\n");

    try
    {
        var guidAndStatusList = SplitResponse(request_response);

        if (guidAndStatusList == null || guidAndStatusList.Count < 2)
        {
            Console.WriteLine("⚠️ SplitResponse returned an invalid result.");
            ErrorLabel.Text = "Unexpected server response format.";
            return;
        }

        Console.WriteLine($"Parsed Response: GUID = {guidAndStatusList[0]}, Status = {guidAndStatusList[1]}");

        if (!Guid.TryParse(guidAndStatusList[0], out Guid parsedGuid))
        {
            Console.WriteLine($"❌ Failed to parse GUID: '{guidAndStatusList[0]}'");
            ErrorLabel.Text = "Login failed: Invalid user ID.";
            return;
        }

        Globals.UserId = parsedGuid;
        Console.WriteLine($"✅ Parsed User ID: {Globals.UserId}");

        if (!int.TryParse(guidAndStatusList[1], out int parsedStatus))
        {
            Console.WriteLine($"❌ Failed to parse Status: '{guidAndStatusList[1]}'");
            ErrorLabel.Text = "Login failed: Invalid status code.";
            return;
        }

        Globals.Status = (Status?)parsedStatus;
        Console.WriteLine($"✅ Parsed Status: {Globals.Status}");

        Application.Current.MainPage = new AppShell();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"🔥 Exception occurred during login process: {ex.Message}");
        Console.WriteLine($"📜 StackTrace:\n{ex.StackTrace}");
        ErrorLabel.Text = $"Login failed: {ex.Message}";
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
        Console.WriteLine($"In the method");
        var parts = guidAndStatus.Split(separator).ToList();
        Console.WriteLine($"Splitting");
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