using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;

namespace Daily3_UI.CustomPopUps;

public class TicketAdminConfirmNewUserPopUp : Popup
{
    public TicketAdminConfirmNewUserPopUp(string username)
    {
        // Resource references - matching TicketAdminPopUp pattern
        Application.Current.Resources.TryGetValue("Black", out var black);
        Application.Current.Resources.TryGetValue("Primary", out var primary);
        Application.Current.Resources.TryGetValue("White", out var white);

        var textColor = (Color)black;
        var buttonBackgroundColor = (Color)primary;
        var backgroundColor = Globals.GetColor("Gray500");

        // Create main grid layout
        var grid = new Grid
        {
            Padding = 20,
            WidthRequest = 300,
            HeightRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Title
                new RowDefinition { Height = GridLength.Auto }, // Message
                new RowDefinition { Height = GridLength.Auto }  // Buttons
            }
        };

        // Title
        var title = new Label
        {
            Text = "Confirm New User",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = textColor,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        };
        grid.Add(title, 0);

        // Message
        var messageLabel = new Label
        {
            Text = $"Are you sure you want to add {username} under you?",
            FontSize = 16,
            TextColor = textColor,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 10, 0, 20)
        };
        grid.Add(messageLabel, 0, 1);

        // Button layout
        var buttonLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 20
        };

        var noButton = new Button
        {
            Text = "No",
            BackgroundColor = buttonBackgroundColor,
            TextColor = textColor,
            WidthRequest = 80,
            HeightRequest = 40,
            Command = new Command(() => Close(false))
        };

        var yesButton = new Button
        {
            Text = "Yes",
            BackgroundColor = buttonBackgroundColor,
            TextColor = textColor,
            WidthRequest = 80,
            HeightRequest = 40,
            Command = new Command(() => Close(true))
        };

        buttonLayout.Children.Add(noButton);
        buttonLayout.Children.Add(yesButton);
        grid.Add(buttonLayout, 0, 2);

        // Wrap grid in Frame - matching TicketAdminPopUp pattern
        Content = new Frame
        {
            Padding = 0,
            CornerRadius = 20,
            BackgroundColor = backgroundColor,
            HasShadow = true,
            Content = grid
        };

        Color = Colors.Transparent;
    }
}