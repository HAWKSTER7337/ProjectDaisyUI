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
    ///     Logging into the application
    ///     This will attempt to log you in based on the password
    ///     and username you give it but its not implemented yet
    /// </summary>
    private void AttemptLogIn(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}