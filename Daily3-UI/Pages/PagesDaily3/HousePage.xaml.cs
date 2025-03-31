using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.CustomPopUps;

namespace Daily3_UI.Pages;

public partial class HousePage : ContentPage
{
    private List<User> _users = new();

    public List<User> Users()
    {
        return _users.OrderBy(user => user.Username).ToList();
    }

    public HousePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        try
        {
            UserLoader.IsRunning = true;
            UserLoader.IsVisible = true;

            _users = await GetUsersAndTicketsUnderHouse.getUsersDaily3();
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

    private void OnTicketButtonClicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        var user = button.BindingContext as User;
        if (user == null) throw new Exception("User is not found");
        var popup = new TicketAdminPopUp<Ticket3>(user.Tickets3.ToList());

        if (this == null) throw new Exception("This is null for some reason");
        if (popup == null) throw new Exception("The Popup is null for some reason");

        this.ShowPopup(popup);
    }
}