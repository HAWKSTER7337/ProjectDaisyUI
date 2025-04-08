using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
        // AdjustLayoutForScreenSize();
    }

    private void ToLogin(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new LogInPage();
    }

    private async void ChangePassword(object sender, EventArgs e)
    {
        var username = Username.Text;
        var oldPassword = OldPassword.Text;
        var newPassword = NewPassword.Text;
        var newPasswordConfirm = NewPassword2.Text;

        if (newPassword != newPasswordConfirm)
        {
            ErrorLabel.Text = "Passwords do not match!";
            return;
        }

        var response = await CreateAccountClient.ChangePassword(username, oldPassword, newPassword);
        ErrorLabel.Text = response;
    }
    
    private void AdjustLayoutForScreenSize()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width;
        var height = DeviceDisplay.MainDisplayInfo.Height;
        double widthScaleFactor = width / 375;  
        double heightScaleFactor = height / 667; 
        double scaleFactor = Math.Min(widthScaleFactor, heightScaleFactor) * 0.5;  
        
        Username.FontSize = 12 * scaleFactor;
        OldPassword.FontSize = 12 * scaleFactor;
        NewPassword.FontSize = 12 * scaleFactor; 
        NewPassword2.FontSize = 12 * scaleFactor;
        
        var buttonScale = scaleFactor * 0.9; 
        ChangePasswordButton.FontSize = 12 * buttonScale; 
        BackToLogInButton.FontSize = 12 * buttonScale;  
        
        ErrorLabel.FontSize = 10 * scaleFactor;  
    }

}