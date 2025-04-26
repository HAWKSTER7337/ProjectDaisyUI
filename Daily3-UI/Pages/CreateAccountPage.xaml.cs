using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class CreateAccountPage : ContentPage
{
    public CreateAccountPage()
    {
        InitializeComponent();
        AdjustLayoutForScreenSize();
    }

    private async void CreateAccount(object sender, EventArgs e)
    {
        ErrorLabel.Text = "";
        var clientResponse = await CreateAccountClient.CreateAccount(Username.Text, Password.Text);
        var successString = "User has been added to the database now you have to wait for approval";
        ErrorLabel.TextColor = clientResponse == successString
            ? Globals.GetColor("SuccessGreen")
            : Globals.GetColor("DailyRed");
        ErrorLabel.Text = clientResponse;
    }

    private async void ToLogin(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    
    private void AdjustLayoutForScreenSize()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width;
        var height = DeviceDisplay.MainDisplayInfo.Height;

        // Use width and height scaling but with much smaller base values
        double widthScaleFactor = width / 375;  // Base width to scale from, 375px is quite standard
        double heightScaleFactor = height / 667; // Base height to scale from, 667px is the height for many devices (e.g., iPhone 6)

        // Taking a more conservative approach for the scale factor
        double scaleFactor = Math.Min(widthScaleFactor, heightScaleFactor) * 0.5;  // Applying a *0.5 to scale down more

        // Adjust font sizes to smaller values
        PageTitle.FontSize = 18 * scaleFactor;  // Reduce title font size significantly
        Username.FontSize = 12 * scaleFactor;   // Reduce input field font sizes
        Password.FontSize = 12 * scaleFactor;   // Same as above for the password field

        // Adjust button font sizes: slightly bigger for better readability
        var buttonScale = scaleFactor * 0.9;  // Slightly larger button font scaling factor
        CreateAccountButton.FontSize = 12 * buttonScale;  // Increase button font size slightly
        BackToLogInButton.FontSize = 12 * buttonScale;  // Increase button font size slightly

        // Adjust the error message font size: make it smaller
        ErrorLabel.FontSize = 10 * scaleFactor;  // Smaller error message font size
    }

}