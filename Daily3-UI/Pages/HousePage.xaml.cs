using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.CustomPopUps;

namespace Daily3_UI.Pages;

public partial class HousePage : ContentPage
{
    private List<User> Users { get; } = new();

    public HousePage()
    {
        InitializeComponent();

        // TODO Make this get the users from the backend
    }

    protected override void OnAppearing()
    {
        UserCollectionView.ItemsSource = Users;
    }

    private void OnTicketButtonClicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;
        var user = button.BindingContext as User;
        if (user == null) throw new Exception("User is not found");
        var popup = new TicketAdminPopUp(user.Tickets.ToList());

        if (this == null) throw new Exception("This is null for some reason");
        if (popup == null) throw new Exception("The Popup is null for some reason");

        this.ShowPopup(popup);
    }
}