using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;

namespace Daily3_UI.CustomPopUps;

public class TicketAdminPopUp : Popup
{
    public TicketAdminPopUp(List<Ticket> tickets)
    {
        // Create layout for popup
        Application.Current.Resources.TryGetValue("Secondary", out var secondary);
        Application.Current.Resources.TryGetValue("Black", out var black);
        Application.Current.Resources.TryGetValue("Primary", out var primary);
        Application.Current.Resources.TryGetValue("neutral3", out var neutral3);

        var neutralThree = (Color)neutral3;
        var backgroundColor = (Color)secondary;
        var textColor = (Color)black;
        var buttonBackgroundColor = (Color)primary;

        Content = new Frame
        {
            Padding = 0,
            CornerRadius = 20,
            BackgroundColor = backgroundColor,
            HasShadow = true,
            Content = new VerticalStackLayout
            {
                Padding = 20,
                WidthRequest = 300,
                HeightRequest = 400,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    // Add a title label
                    new Label
                    {
                        Text = "Ticket Details",
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = textColor,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start
                    },

                    // Adding the collectors view to display the tickets
                    new CollectionView
                    {
                        ItemsSource = tickets,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                        ItemTemplate = new DataTemplate(() =>
                        {
                            var grid = new Grid
                            {
                                Padding = 10,
                                RowDefinitions =
                                {
                                    new RowDefinition { Height = GridLength.Auto },
                                    new RowDefinition { Height = GridLength.Auto }
                                }
                            };

                            // Adding the first Label for the tickets
                            var label1 = new Label
                            {
                                FontSize = 16
                            };
                            label1.SetBinding(Label.TextProperty, "TextFormat");
                            // Getting the Status to Color Converter
                            Application.Current.Resources.TryGetValue("StatusToColor",
                                out var statusToColorConverter);
                            label1.SetBinding(Label.TextColorProperty,
                                new Binding("WinningStatus", BindingMode.OneWay,
                                    (IValueConverter)statusToColorConverter));

                            grid.Add(label1, 0);

                            // Adding the details format label
                            var label2 = new Label
                            {
                                FontSize = 14,
                                TextColor = neutralThree
                            };
                            label2.SetBinding(Label.TextProperty, "DetailsFormat");

                            grid.Add(label2, 0, 1);

                            return grid;
                        })
                    },

                    // Add a Close button
                    new Button
                    {
                        Text = "Close",
                        BackgroundColor = buttonBackgroundColor,
                        TextColor = textColor,
                        WidthRequest = 100,
                        HorizontalOptions = LayoutOptions.Center,
                        Command = new Command(() => Close())
                    }
                }
            }
        };

        Color = Colors.Transparent;
    }
}