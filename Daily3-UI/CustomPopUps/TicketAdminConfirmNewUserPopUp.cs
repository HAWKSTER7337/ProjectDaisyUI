using CommunityToolkit.Maui.Views;

namespace Daily3_UI.CustomPopUps;

public class TicketAdminConfirmNewUserPopUp : Popup
{
    public TicketAdminConfirmNewUserPopUp(string username)
    {
        var label = new Label
        {
            Text = $"Are you sure you want to add {username} under you?",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10)
        };

        var yesButton = new Button
        {
            Text = "Yes",
            HorizontalOptions = LayoutOptions.EndAndExpand
        };

        var noButton = new Button
        {
            Text = "No",
            HorizontalOptions = LayoutOptions.StartAndExpand
        };

        yesButton.Clicked += (s, e) => { Close(true); };

        noButton.Clicked += (s, e) => { Close(false); };

        var buttonLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            Children = { noButton, yesButton }
        };

        Content = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = new Thickness(20),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { label, buttonLayout }
        };
    }
}