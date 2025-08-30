using CommunityToolkit.Maui.Views;
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
        // Clear previous errors and show loading state
        ErrorLabel.Text = "";
        ErrorLabel.TextColor = Colors.Red;
        LoginButton.IsEnabled = false;
        LoginButton.Text = "Logging In...";
        
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Username.Text) || string.IsNullOrWhiteSpace(Password.Text))
            {
                ShowError("Please enter both username and password.", LoginErrorType.InvalidCredentials);
                return;
            }

            var loginResult = await VerifyUserClient.VerifyUser(Username.Text, Password.Text);
            
            if (loginResult.IsSuccess)
            {
                // Success - update globals and navigate
                Globals.UserId = loginResult.UserGuid!.Value;
                Globals.Status = (Status?)loginResult.UserStatus!.Value;
                
                // Show success message briefly
                ShowSuccess("Login successful! Redirecting...");
                
                // Navigate to appropriate page
                var nextPage = Globals.Status > Status.User ? "//Entrants" : "//Buy3";
                await Shell.Current.GoToAsync(nextPage, true);
            }
            else
            {
                // Show appropriate error message
                ShowError(loginResult.Message, loginResult.ErrorType);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            ShowError("An unexpected error occurred. Please try again.", LoginErrorType.Unknown);
        }
        finally
        {
            // Restore button state
            LoginButton.IsEnabled = true;
            LoginButton.Text = "Log In";
        }
    }

    /// <summary>
    ///     Shows an error message with appropriate styling based on error type
    /// </summary>
    private void ShowError(string message, LoginErrorType errorType)
    {
        ErrorLabel.Text = message;
        
        // Set appropriate color based on error type
        ErrorLabel.TextColor = errorType switch
        {
            LoginErrorType.InvalidCredentials => Colors.Orange,
            LoginErrorType.AccountPending => Colors.Blue,
            LoginErrorType.NetworkError => Colors.Red,
            LoginErrorType.Timeout => Colors.Orange,
            LoginErrorType.ServerError => Colors.Red,
            _ => Colors.Red
        };
        
        // Add icon or additional styling for specific error types
        if (errorType == LoginErrorType.AccountPending)
        {
            ErrorLabel.Text = "⏳ " + message;
        }
        else if (errorType == LoginErrorType.NetworkError)
        {
            ErrorLabel.Text = "📡 " + message;
        }
        else if (errorType == LoginErrorType.Timeout)
        {
            ErrorLabel.Text = "⏰ " + message;
        }
    }

    /// <summary>
    ///     Shows a success message
    /// </summary>
    private void ShowSuccess(string message)
    {
        ErrorLabel.Text = "✅ " + message;
        ErrorLabel.TextColor = Colors.Green;
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

    private void OnBackgroundTapped(object sender, EventArgs args)
    {
        Username.Unfocus();
        Password.Unfocus();
        
        KeyboardHelper.HideKeyboard();
    }
}