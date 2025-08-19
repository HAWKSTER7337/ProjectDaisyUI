using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.CustomPopUps;

namespace Daily3_UI.Pages;

public partial class HouseAddUsersPage : ContentPage
{
    private List<User> _users = new();

    public List<User> Users()
    {
        return _users.OrderBy(user => user.Username).ToList();
    }

    public HouseAddUsersPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        await LoadScreen();
    }

    private async Task LoadScreen()
    {
        try
        {
            UserLoader.IsRunning = true;
            UserLoader.IsVisible = true;

            _users = await GetUsersAndTicketsUnderHouse.GetUnverifiedUsers();

            UserCollectionView.ItemsSource = Users();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            UserLoader.IsRunning = false;
            UserLoader.IsVisible = false;
        }
    }

    private async void OnHousePageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void RequestToAddUser(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        var user = button.BindingContext as User;

        var isAddingUserUnderThem =
            await Application.Current.MainPage.ShowPopupAsync(new TicketAdminConfirmNewUserPopUp(user.Username)) as bool
                ?;

        if (isAddingUserUnderThem != true) return;
        await GetUsersAndTicketsUnderHouse.AddUserUnderHouse(user.UserId);
        await LoadScreen();
    }
}