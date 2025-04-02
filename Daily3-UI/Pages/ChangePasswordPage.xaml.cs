using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
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
}