using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.Enums;
using Daily3_UI.Pages.PagesDaily3;

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
    
    public void SetErrorLabel(string error)
    {
        ErrorLabel.Text = error;
    }
    
    public void ChangeErrorLabelColor(Color color)
    {
        ErrorLabel.TextColor = color;
    }
    
    private async void CheckOut(object sender, EventArgs e)
    {
        var validTickets = AddTicketsInSpecifiedIntervalToCart();
        if (!validTickets) return;
        await OpenTicketPopupPageAsync();
    }

    private async void TakeUserToTicketHistoryPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("HistoryPage");
    }

    private async void TakeUserToWinningNumberPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("WinningNumbers4");
    }
    
    private async void ChangeLotto(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Buy3");
    }

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
    
    private bool AddTicketsInSpecifiedIntervalToCart()
    {
        var beginningDate = DatePicker.Date;
        var endDate = SecondDatePicker.Date;
        while (DateTime.Compare(beginningDate, endDate) <= 0)
        {
            var ticketValid = AddTicketToCart(beginningDate.ToString("yyyy-MM-dd"));
            if (!ticketValid) return false;
            beginningDate = beginningDate.AddDays(1);
        }

        return true;
    }

    /// <summary>
    ///     Adds the ticket to the queue of the
    ///     tickets you are planning to buy
    /// </summary>
    private bool AddTicketToCart(string date)
    {
        try
        {
            var number1 = int.TryParse(Number1Entry.Text, out var n1) ? n1 : (int?)null;
            var number2 = int.TryParse(Number2Entry.Text, out var n2) ? n2 : (int?)null;
            var number3 = int.TryParse(Number3Entry.Text, out var n3) ? n3 : (int?)null;
            var number4 = int.TryParse(Number4Entry.Text, out var n4) ? n3 : (int?)null;
            
            var ticket = new Ticket4
            {
                Number1 = number1,
                Number2 = number2,
                Number3 = number3,
                Number4 = number4,
                Price = double.Parse(PriceButton.Text),
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
                return false;
            }
            
            ShoppingCart.Add(ticket);
            return true;
        }
        catch (NullReferenceException exception)
        {
            Console.WriteLine(exception);
            ErrorLabel.TextColor = Globals.GetColor("DailyRed");
            ErrorLabel.Text = "A Specification Has Not Been Set";
            return false;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            ErrorLabel.TextColor = Globals.GetColor("DailyRed");
            ErrorLabel.Text = "Something is fishy here....";
            return false;
        }
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

    public async Task OpenTicketPopupPageAsync()
    {
        var ticketPopupPage = new TicketPopupPage(ShoppingCart, this);
        await Shell.Current.CurrentPage.ShowPopupAsync(ticketPopupPage);
    }
    
    public void ClearShoppingCart()
    {
        ShoppingCart.Clear();
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
        var buttonFontSize = screenWidth * 0.05;
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

        Number1Entry.FontSize = pickerFontSize;
        Number2Entry.FontSize = pickerFontSize;
        Number3Entry.FontSize = pickerFontSize;
        Number4Entry.FontSize = pickerFontSize;
        PriceButton.FontSize = pickerFontSize;

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

        BuyTicketsButton.FontSize = buttonFontSize;
        HistoryButton.FontSize = buttonFontSize;
        WinningNumbersButton.FontSize = buttonFontSize;
    }

    private void OnNumber1TextChanged(object sender, TextChangedEventArgs e)
    {
        var currentIsEmpty = string.IsNullOrEmpty(Number1Entry.Text);
        var nextIsEmpty = string.IsNullOrEmpty(Number2Entry.Text);
        if (!currentIsEmpty && nextIsEmpty)
        {
            Number2Entry.Focus();
        }
        else if (!currentIsEmpty)
        {
            Number1Entry.Unfocus();
        }
    }

    private void OnNumber2TextChanged(object sender, TextChangedEventArgs e)
    {
        var currentIsEmpty = string.IsNullOrEmpty(Number2Entry.Text);
        var nextIsEmpty = string.IsNullOrEmpty(Number3Entry.Text);
        if (!currentIsEmpty && nextIsEmpty)
        {
            Number3Entry.Focus();
        }
        else if (!currentIsEmpty)
        {
            Number2Entry.Unfocus();
        }
    }
    
    private void OnNumber3TextChanged(object sender, TextChangedEventArgs e)
    {
        var currentIsEmpty = string.IsNullOrEmpty(Number3Entry.Text);
        var nextIsEmpty = string.IsNullOrEmpty(Number4Entry.Text);
        if (!currentIsEmpty && nextIsEmpty)
        {
            Number4Entry.Focus();
        }
        else if (!currentIsEmpty)
        {
            Number3Entry.Unfocus();
        }
    }

    private void OnNumber4TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Number4Entry.Text))
            Number4Entry.Unfocus();
    }

    private async void OnPriceButtonClicked(object sender, EventArgs e)
    {
        var priceOptions = new List<string>();
        priceOptions.Add("1.00");
        priceOptions.Add("0.50");
        priceOptions.Add("0.25");

        for (double price = 0.05; price <= 1.00; price += 0.05)
        {
            priceOptions.Add(price.ToString("0.00"));
        }

        var result = await DisplayActionSheet("Select a Price", "Cancel", null, priceOptions.ToArray());


        if (result != null && result != "Cancel")
        {
            PriceButton.Text = result;
        }
    }

    private void OnNumberEntryFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = string.Empty;
        }
    }
}