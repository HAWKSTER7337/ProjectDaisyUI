using Daily3_UI.Pages;

namespace Daily3_UI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        NavigateToLogin();
    }

    private async void NavigateToLogin()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}