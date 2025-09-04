using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Pages;

namespace Daily3_UI.CustomPopUps;

public class TicketAdminPopUp : Popup
{
    public TicketAdminPopUp(List<Ticket> tickets)
    {
        // Resource references
        Application.Current.Resources.TryGetValue("Black", out var black);
        Application.Current.Resources.TryGetValue("Primary", out var primary);
        Application.Current.Resources.TryGetValue("neutral3", out var neutral3);

        var neutralThree = (Color)neutral3;
        var backgroundColor = Globals.GetColor("Gray500");
        var textColor = (Color)black;
        var buttonBackgroundColor = (Color)primary;
        Application.Current.Resources["StatusToColor"] = new StatusToColorConverter();

        // Get screen dimensions for responsive sizing
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        
        // Calculate responsive dimensions
        var popupWidth = Math.Min(screenWidth * 0.85, 500); // 85% of screen width, max 500px
        var popupHeight = Math.Min(screenHeight * 0.7, 600); // 70% of screen height, max 600px
        
        // Ensure minimum sizes for usability
        popupWidth = Math.Max(popupWidth, 350); // Minimum 350px width
        popupHeight = Math.Max(popupHeight, 400); // Minimum 400px height

        // Create layout using Grid instead of VerticalStackLayout
        var grid = new Grid
        {
            Padding = 20,
            WidthRequest = popupWidth,
            HeightRequest = popupHeight,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Title
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // CollectionView
                new RowDefinition { Height = GridLength.Auto } // Button
            }
        };

        // Title
        var title = new Label
        {
            Text = "Ticket Details",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = textColor,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        };
        grid.Add(title, 0);

        // CollectionView
        var collectionView = new CollectionView
        {
            ItemsSource = tickets,
            VerticalScrollBarVisibility = ScrollBarVisibility.Always,
            ItemTemplate = new DataTemplate(() =>
            {
                var cellGrid = new Grid
                {
                    Padding = new Thickness(15, 12, 15, 12),
                    Margin = new Thickness(0, 2, 0, 2),
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                var label1 = new Label 
                { 
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 4)
                };
                label1.SetBinding(Label.TextProperty, "TextFormat");
                Application.Current.Resources.TryGetValue("StatusToColor", out var statusToColorConverter);
                label1.SetBinding(Label.TextColorProperty,
                    new Binding("WinningStatus", BindingMode.OneWay, (IValueConverter)statusToColorConverter));

                var label2 = new Label
                {
                    FontSize = 15,
                    TextColor = neutralThree,
                    LineBreakMode = LineBreakMode.WordWrap
                };
                label2.SetBinding(Label.TextProperty, "DetailsFormatWithDateTime");

                cellGrid.Add(label1, 0);
                cellGrid.Add(label2, 0, 1);

                return cellGrid;
            })
        };
        grid.Add(collectionView, 0, 1);

        // Close Button
        var closeButton = new Button
        {
            Text = "Close",
            BackgroundColor = buttonBackgroundColor,
            TextColor = textColor,
            WidthRequest = 100,
            HorizontalOptions = LayoutOptions.Center,
            Command = new Command(() => Close())
        };
        grid.Add(closeButton, 0, 2);

        // Wrap grid in Frame
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