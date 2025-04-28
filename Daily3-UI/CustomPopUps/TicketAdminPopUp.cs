using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Pages;
using Daily3_UI.Pages.PagesDaily3;

namespace Daily3_UI.CustomPopUps;

public class TicketAdminPopUp<T> : Popup
    where T : Ticket, new()
{
    public TicketAdminPopUp(List<T> tickets)
    {
        // Resource references
        Application.Current.Resources.TryGetValue("Black", out var black);
        Application.Current.Resources.TryGetValue("Primary", out var primary);
        Application.Current.Resources.TryGetValue("neutral3", out var neutral3);

        var ticket = new T();
        var neutralThree = (Color)neutral3;
        var backgroundColor = ticket.TicketColorTheme;
        var textColor = (Color)black;
        var buttonBackgroundColor = (Color)primary;
        Application.Current.Resources["StatusToColor"] = new StatusToColorConverter();

        // Create layout using Grid instead of VerticalStackLayout
        var grid = new Grid
        {
            Padding = 20,
            WidthRequest = 300,
            HeightRequest = 400,
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
                    Padding = 10,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                var label1 = new Label { FontSize = 16 };
                label1.SetBinding(Label.TextProperty, "TextFormat");
                Application.Current.Resources.TryGetValue("StatusToColor", out var statusToColorConverter);
                label1.SetBinding(Label.TextColorProperty,
                    new Binding("WinningStatus", BindingMode.OneWay, (IValueConverter)statusToColorConverter));

                var label2 = new Label
                {
                    FontSize = 14,
                    TextColor = neutralThree
                };
                label2.SetBinding(Label.TextProperty, "DetailsFormat");

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