using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.Enums;

namespace Daily3_UI.Pages;

public partial class BuyTicketsDaily4 : ContentPage
{
    public BuyTicketsDaily4()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Where all tickets are stored
    /// </summary>
    private List<Ticket4> ShoppingCart { get; } = new();

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
        if (selectedButton is not null) selectedButton.BackgroundColor = Globals.GetColor("Primary");
        selectedButton = senderButton;
        selectedButton.BackgroundColor = Globals.GetColor("Selected");
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
    ///     Adds tickets to the shopping cart for the specified date range.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event data.</param>
    private void AddTicketsInSpecifiedIntervalToCart(object sender, EventArgs e)
    {
        var beginningDate = DatePicker.Date;
        var endDate = SecondDatePicker.Date;
        while (DateTime.Compare(beginningDate, endDate) <= 0)
        {
            AddTicketToCart(beginningDate.ToString("yyyy-MM-dd"));
            beginningDate = beginningDate.AddDays(1);
        }
    }

    /// <summary>
    ///     Adds the ticket to the queue of the
    ///     tickets you are planning to buy
    /// </summary>
    private void AddTicketToCart(string date)
    {
        try
        {
            var ticket = new Ticket4
            {
                Number1 = short.Parse(Number1.SelectedItem.ToString()),
                Number2 = short.Parse(Number2.SelectedItem.ToString()),
                Number3 = short.Parse(Number3.SelectedItem.ToString()),
                Number4 = short.Parse(Number4.SelectedItem.ToString()),
                Price = double.Parse(Price.SelectedItem.ToString()),
                Type = GetTicketTypeFromString(BetTypeSelcted.Text),
                TimeOfDay = GetTodFromString(TimeOfDaySelected.Text),
                Date = date
            };

            // Checking for missing fields 
            var emptyFieldMessage = ticket.MissingMessage();
            if (emptyFieldMessage is not null)
            {
                ErrorLabel.TextColor = Globals.GetColor("DailyRed");
                ErrorLabel.Text = emptyFieldMessage;
                return;
            }

            // Adding Values to the shopping cart
            ShoppingCart.Add(ticket);
            //ErrorLabel.TextColor = (Color)Application.Current.Resources["DailyGreen"];
            ErrorLabel.TextColor = Globals.GetColor("SuccessGreen");
            ErrorLabel.Text = $"{ShoppingCart.Count} Ticket(s) in cart.";
        }
        catch (NullReferenceException exception)
        {
            Console.WriteLine(exception);
            ErrorLabel.TextColor = Globals.GetColor("DailyRed");
            ErrorLabel.Text = "A Specification Has Not Been Set";
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            ErrorLabel.TextColor = Globals.GetColor("DailyRed");
            ErrorLabel.Text = "Something is fishy here....";
        }
    }

    /// <summary>
    ///     Checkout and send all the tickets
    ///     to the server
    /// </summary>
    private async void CheckOut(object sender, EventArgs e)
    {
        var copyShoppingCart = new List<Ticket4>(ShoppingCart);
        ShoppingCart.Clear();

        var errorMessage = await BuyTicketClient.BuyTicketsDaily4(copyShoppingCart);

        ErrorLabel.TextColor = errorMessage != "Tickets sent successfully"
            ? Globals.GetColor("DailyRed")
            : Globals.GetColor("SuccessGreen");
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

    public async void OpenTicketPopupPageAsync(object sender, EventArgs e)
    {
        var ticketPopupPage = new TicketPopupPage(ShoppingCart);
        await Shell.Current.CurrentPage.ShowPopupAsync(ticketPopupPage);
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        var screenWidth = Width;
        var screenHeight = Height;

        // Scale values for elements
        var borderWidth = screenWidth * 0.8;
        var numberBorderWidth = borderWidth / 6;
        var labelFontSize = screenWidth * 0.03;
        var pickerFontSize = screenWidth * 0.04;
        var buttonWidth = screenWidth * 0.25;
        var buttonHeight = screenHeight * 0.06;
        var buttonFontSize = screenWidth * 0.03;
        var errorLabelFont = screenWidth * 0.05;

        // Adjust elements accordingly
        Number1Border.WidthRequest = Number1Border.HeightRequest = numberBorderWidth;
        Number2Border.WidthRequest = Number2Border.HeightRequest = numberBorderWidth;
        Number3Border.WidthRequest = Number3Border.HeightRequest = numberBorderWidth;
        Number4Border.WidthRequest = Number4Border.HeightRequest = numberBorderWidth;

        SelectPaymentLabel.FontSize = labelFontSize;
        SelectBetTypeLabel.FontSize = labelFontSize;
        SelectDrawLabel.FontSize = labelFontSize;
        SelectDateLabel.FontSize = labelFontSize;
        ErrorLabel.FontSize = errorLabelFont;

        Number1.FontSize = pickerFontSize;
        Number2.FontSize = pickerFontSize;
        Number3.FontSize = pickerFontSize;
        Number4.FontSize = pickerFontSize;
        Price.FontSize = pickerFontSize;

        StraightButton.WidthRequest = buttonWidth;
        StraightButton.HeightRequest = buttonHeight;
        StraightButton.FontSize = buttonFontSize;

        BoxButton.WidthRequest = buttonWidth;
        BoxButton.HeightRequest = buttonHeight;
        BoxButton.FontSize = buttonFontSize;

        TwoWayButton.WidthRequest = buttonWidth;
        TwoWayButton.HeightRequest = buttonHeight;
        TwoWayButton.FontSize = buttonFontSize;

        OneOffButton.WidthRequest = buttonWidth;
        OneOffButton.HeightRequest = buttonHeight;
        OneOffButton.FontSize = buttonFontSize;

        WheelButton.WidthRequest = buttonWidth;
        WheelButton.HeightRequest = buttonHeight;
        WheelButton.FontSize = buttonFontSize;

        MiddayButton.WidthRequest = buttonWidth;
        MiddayButton.HeightRequest = buttonHeight;
        MiddayButton.FontSize = buttonFontSize;

        EveningButton.WidthRequest = buttonWidth;
        EveningButton.HeightRequest = buttonHeight;
        EveningButton.FontSize = buttonFontSize;

        BothButton.WidthRequest = buttonWidth;
        BothButton.HeightRequest = buttonHeight;
        BothButton.FontSize = buttonFontSize;

        OpenTicketCartButton.WidthRequest = buttonWidth;
        OpenTicketCartButton.HeightRequest = buttonHeight;
        OpenTicketCartButton.FontSize = buttonFontSize;

        AddTicketsToCartButton.WidthRequest = buttonWidth;
        AddTicketsToCartButton.HeightRequest = buttonHeight;
        AddTicketsToCartButton.FontSize = buttonFontSize;

        ButTicketsButton.WidthRequest = buttonWidth;
        ButTicketsButton.HeightRequest = buttonHeight;
        ButTicketsButton.FontSize = buttonFontSize;
    }
}