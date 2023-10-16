using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.Enums;

namespace Daily3_UI.Pages;

public partial class BuyTickets : ContentPage
{
    public BuyTickets()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Where all tickets are stored
    /// </summary>
    private List<Ticket> ShoppingCart { get; } = new();

    /// <summary>
    ///     The bet type For the currently selected code
    /// </summary>
    private Button? BetTypeSelcted;

    /// <summary>
    ///     TimeOfDay for the bet
    /// </summary>
    private Button? TimeOfDaySelected;

    /// <summary>
    ///     Function to select a button from a group of buttons
    /// </summary>
    private void SelectTypeOrTimeOfDay(object sender, EventArgs e, ref Button selectedButton)
    {
        var senderButton = (Button)sender;
        if (selectedButton is not null) selectedButton.BackgroundColor = GetColor("Primary");
        selectedButton = senderButton;
        selectedButton.BackgroundColor = GetColor("Selected");
    }

    /// <summary>
    ///     Trys to get color from static resources
    ///     returns white if the color is not found
    /// </summary>
    private Color GetColor(string colorName)
    {
        var resourceColor = Application.Current.Resources.TryGetValue(colorName, out var value) && value is Color color
            ? color
            : Color.FromRgb(0, 0, 0);
        return resourceColor;
    }

    private void SelectType(object sender, EventArgs e)
    {
        SelectTypeOrTimeOfDay(sender, e, ref BetTypeSelcted);
    }

    private void SelectTimeOfDay(object sender, EventArgs e)
    {
        SelectTypeOrTimeOfDay(sender, e, ref TimeOfDaySelected);
    }

    /// <summary>
    ///     Allows you to only pick for
    ///     times that have not been called yet.
    ///     Meaning you can't buy tickets fot lottery
    ///     numbers that have already been called
    /// </summary>
    private void ProperDatePicker(object sender, DateChangedEventArgs e)
    {
        var selectedDate = e.NewDate;
        var currentDate = DateTime.Now.Date;

        if (selectedDate < currentDate) DatePicker.Date = currentDate;
    }

    /// <summary>
    ///     Adds the ticket to the queue of the
    ///     tickets you are planning to buy
    /// </summary>
    private void AddTicketToCart(object sender, EventArgs e)
    {
        try
        {
            var ticket = new Ticket
            {
                Number1 = short.Parse(Number1.SelectedItem.ToString()),
                Number2 = short.Parse(Number2.SelectedItem.ToString()),
                Number3 = short.Parse(Number3.SelectedItem.ToString()),
                Price = double.Parse(Price.SelectedItem.ToString()),
                Type = GetTicketTypeFromString(BetTypeSelcted.Text),
                TimeOfDay = GetTodFromString(TimeOfDaySelected.Text),
                Date = DatePicker.Date.ToString("yyyy-MM-dd")
            };

            // Checking for missing fields 
            var emptyFieldMessage = ticket.MissingMessage();
            if (emptyFieldMessage is not null)
            {
                ErrorLabel.Text = emptyFieldMessage;
                return;
            }

            // Adding Values to the shopping cart
            ShoppingCart.Add(ticket);
            ErrorLabel.TextColor = (Color)Application.Current.Resources["DailyGreen"];
            ErrorLabel.Text = $"{ShoppingCart.Count} Ticket(s) in cart.";
        }
        catch (NullReferenceException exception)
        {
            Console.WriteLine(exception);
            ErrorLabel.Text = "A Specification Has Not Been Set";
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            ErrorLabel.Text = "Something is fishy here....";
        }
    }

    /// <summary>
    ///     Checkout and send all of the tickets
    ///     to the server
    /// </summary>
    private async void CheckOut(object sender, EventArgs e)
    {
        var copyShoppingCart = new List<Ticket>(ShoppingCart);
        ShoppingCart.Clear();

        var errorMessage = "";

        foreach (var ticket in copyShoppingCart)
        {
            errorMessage = await BuyTicketClient.BuyTicket(ticket);
            if (errorMessage != "") break;
            await Task.Delay(TimeSpan.FromMilliseconds(200));
        }

        ErrorLabel.TextColor = errorMessage != ""
            ? (Color)Application.Current.Resources["DailyRed"]
            : (Color)Application.Current.Resources["DailyGreen"];
        ErrorLabel.Text = errorMessage != "" ? errorMessage : "Tickets Sent";
    }

    private TOD? GetTodFromString(string text)
    {
        return text switch
        {
            "Midday" => TOD.Midday,
            "Evening" => TOD.Evening,
            "Both" => TOD.Both,
            _ => null
        };
    }

    private TicketType? GetTicketTypeFromString(string text)
    {
        return text switch
        {
            "Straight" => TicketType.Straight,
            "Box" => TicketType.Box,
            "2-Way" => TicketType.TwoWay,
            "1-Off" => TicketType.OneOff,
            "Wheel" => TicketType.Wheel,
            _ => null
        };
    }
}