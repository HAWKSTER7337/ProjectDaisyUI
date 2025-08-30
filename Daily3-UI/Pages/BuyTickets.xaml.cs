using System.Globalization;
using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Enums;

namespace Daily3_UI.Pages.PagesDaily3;

public partial class BuyTickets : ContentPage
{
    public BuyTickets()
    {
        InitializeComponent();
        
        // Ensure BindingContext is set after InitializeComponent
        BindingContext = this;
        
        // Initialize default values
        IsUser = true;
        IsDaily3 = true;
    } 

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Use Device.BeginInvokeOnMainThread to ensure the page is fully loaded
        // This helps prevent crashes on iOS when the page dimensions aren't ready
        Device.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                // Add a small delay to ensure the page is fully rendered
                await Task.Delay(100);
                MoveNumberOfDays();
            }
            catch (Exception ex)
            {
                // Log the error but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Error in MoveNumberOfDays: {ex.Message}");
            }
        });
    }

    private List<Ticket> ShoppingCart { get; } = new();

    private Button? BetTypeSelcted;

    private Button? TimeOfDaySelected;

    private bool _isDaily3 = true;
    private bool _isUser = true; // Default to true for level 0 users

    private double PriceLimit => _isDaily3 ? 20.0 : 4.0;

    private bool IsDaily3
    {
        get => _isDaily3;
        set
        {
            _isDaily3 = value;
            if (IsDaily3)
            {
                BackgroundColor = Globals.GetColor("Secondary");
                Daily3Numbers.IsVisible = true;
                Daily4Numbers.IsVisible = false;
            }
            else if (IsDaily4)
            {
                BackgroundColor = Globals.GetColor("SecondaryDaily4");
                Daily3Numbers.IsVisible = false; 
                Daily4Numbers.IsVisible = true;
            }
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsDaily4));
        }
    }

    public bool IsDaily4 => !IsDaily3;
    
    public bool IsUser
    {
        get => _isUser;
        set
        {
            _isUser = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsNumberOfDaysMode
    {
        get => true; // Always show the number of days mode for now
        set
        {
            OnPropertyChanged();
        }
    }
    

    public void SetErrorLabel(string error)
    {
        ErrorLabel.Text = error;
    }

    public void ChangeErrorLabelColor(Color color)
    {
        ErrorLabel.TextColor = color;
    }

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

    private MonthAndDay GetDateAndMonth(DateTime date)
    {
        return new MonthAndDay()
        {
            Month = date.Month,
            Day = date.Day,
        };
    }

    private DateTime UpdateMonthAndDay(MonthAndDay monthAndDay)
    {
        var format = "MM/dd/yyyy";
        var dateString = $"{monthAndDay.Month:D2}/{monthAndDay.Day:D2}/{Globals.CurrentYear}";

        if (DateTime.TryParseExact(
                dateString,
                format,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out DateTime parsedDate))
        {
            return parsedDate;
        }

        throw new FormatException($"Failed to parse date from input: {dateString}");
    }

    private bool AddTicketsFromPreSetToCart()
    {
        var beginningDate = DateTime.Today;
        var endDate = beginningDate.AddDays(_currentNumberOfDaysSelected - 1);
        return AddToCart(beginningDate, endDate);
    }

    private bool AddToCart(DateTime startTime, DateTime endTime)
    {
        while (DateTime.Compare(startTime, endTime) <= 0)
        {
            var ticketValid = AddTicketToCart(startTime.ToString("yyyy-MM-dd"));
            if (!ticketValid) return false;
            startTime = startTime.AddDays(1);
        }

        return true;
    }

    private async void TakeUserToTicketHistoryPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("HistoryPage");
    }

    private async void TakeUserToWinningNumberPage(object sender, EventArgs e)
    {
        if (IsDaily3)
            await Shell.Current.GoToAsync("WinningNumbers3");
        else
            await Shell.Current.GoToAsync("WinningNumbers4");
    }

    private void ChangeLotto(object sender, EventArgs e)
    {
        IsDaily3 = !IsDaily3;
        OnPriceSet(sender, e);
    }

    private static DateTime MiddayCutOff => DateTime.Today.AddHours(12).AddMinutes(39);
    private static DateTime EveningCutOff => DateTime.Today.AddHours(19).AddMinutes(08);

    private bool AddTicketToCart(string date)
    {
        try
        {
            var ticket = BuildTicketFromData(date);

            // Checking for missing fields 
            var emptyFieldMessage = ticket.MissingMessage();
            if (emptyFieldMessage is not null)
            {
                ErrorLabel.TextColor = Globals.GetColor("DailyRed");
                ErrorLabel.Text = emptyFieldMessage;
                return false;
            }

            if (ticket.TimeOfDay == TOD.Both)
            {
                SplitBothTicketIntoTwo(ticket);
                return true;
            }

            HandleTicketDate(ticket);
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

    private Ticket BuildTicketFromData(string date)
    {
        Ticket ticket;
        if (IsDaily3)
        {
            var number1 = int.TryParse(Number1Entry.Text, out var n1) ? n1 : (int?)null;
            var number2 = int.TryParse(Number2Entry.Text, out var n2) ? n2 : (int?)null;
            var number3 = int.TryParse(Number3Entry.Text, out var n3) ? n3 : (int?)null;

            ticket = new Ticket3
            {
                Number1 = number1,
                Number2 = number2,
                Number3 = number3,
                Price = double.Parse(PriceEntry.Text),
                Type = GetTicketTypeFromString(BetTypeSelcted.Text),
                TimeOfDay = GetTodFromString(TimeOfDaySelected.Text),
                Date = date
            };
        }
        else
        {
            var number1 = int.TryParse(Number1Entry4.Text, out var n1) ? n1 : (int?)null;
            var number2 = int.TryParse(Number2Entry4.Text, out var n2) ? n2 : (int?)null;
            var number3 = int.TryParse(Number3Entry4.Text, out var n3) ? n3 : (int?)null;
            var number4 = int.TryParse(Number4Entry4.Text, out var n4) ? n4 : (int?)null;

            ticket = new Ticket4
            {
                Number1 = number1,
                Number2 = number2,
                Number3 = number3,
                Number4 = number4,
                Price = double.Parse(PriceEntry.Text),
                Type = GetTicketTypeFromString(BetTypeSelcted.Text),
                TimeOfDay = GetTodFromString(TimeOfDaySelected.Text),
                Date = date
            };
        }

        return ticket;
    }

    private void HandleTicketDate(Ticket ticket)
    {
        if ((ticket.TimeOfDay == TOD.Midday && DateTime.Now > MiddayCutOff) ||
            (ticket.TimeOfDay == TOD.Evening && DateTime.Now > EveningCutOff))
            ticket.AddNumberOfDaysToTicketDate(1);
    }

    private void SplitBothTicketIntoTwo(Ticket ticket)
    {
        var otherTicket = ticket.ShallowCopy();

        var currentTime = DateTime.Now;

        if (currentTime < MiddayCutOff)
        {
            ticket.TimeOfDay = TOD.Midday;
            otherTicket.TimeOfDay = TOD.Evening;
        }
        else if (currentTime < EveningCutOff)
        {
            ticket.TimeOfDay = TOD.Evening;

            otherTicket.TimeOfDay = TOD.Midday;
            otherTicket.AddNumberOfDaysToTicketDate(1);
        }
        else
        {
            ticket.TimeOfDay = TOD.Midday;
            ticket.AddNumberOfDaysToTicketDate(1);

            otherTicket.TimeOfDay = TOD.Evening;
            otherTicket.AddNumberOfDaysToTicketDate(1);
        }

        ShoppingCart.Add(ticket);
        ShoppingCart.Add(otherTicket);
    }

    private async void CheckOut(object sender, EventArgs e)
    {
        ErrorLabel.Text = "";
        var validTickets = AddTicketsFromPreSetToCart();
        if (!validTickets) return;
        await OpenTicketPopupPageAsync();
    }

    private void ClearNumbers(object sender, EventArgs e)
    {
        Number1Entry.Text = string.Empty;
        Number2Entry.Text = string.Empty;
        Number3Entry.Text = string.Empty;

        Number1Entry4.Text = string.Empty;
        Number2Entry4.Text = string.Empty;
        Number3Entry4.Text = string.Empty;
        Number4Entry4.Text = string.Empty;
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
            "Wheel" => TicketType.Wheel,
            _ => null
        };
    }

    private async Task OpenTicketPopupPageAsync()
    {
        var ticketPopupPage = new TicketPopupPage(ShoppingCart, this);
        await Shell.Current.CurrentPage.ShowPopupAsync(ticketPopupPage);
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        // Check if Width and Height are valid before proceeding
        if (Width <= 0 || Height <= 0 || double.IsNaN(Width) || double.IsNaN(Height))
        {
            return; // Exit early if dimensions are not yet available
        }

        var screenWidth = Width;
        var screenHeight = Height;

        // Scale values for elements
        var borderWidth = screenWidth * 0.8;
        var numberBorderWidth = borderWidth / 6;
        var labelFontSize = screenWidth * 0.02;
        var pickerFontSize = screenWidth * 0.04;
        var buttonWidth = screenWidth * 0.25;
        var buttonHeight = screenHeight * 0.06;
        var buttonFontSize = screenWidth * 0.05;
        var errorLabelFont = screenWidth * 0.05;

        // Adjust elements accordingly - add null checks for safety
        if (Number1Border != null) Number1Border.WidthRequest = Number1Border.HeightRequest = numberBorderWidth;
        if (Number2Border != null) Number2Border.WidthRequest = Number2Border.HeightRequest = numberBorderWidth;
        if (Number3Border != null) Number3Border.WidthRequest = Number3Border.HeightRequest = numberBorderWidth;

        if (Number1BorderD1 != null) Number1BorderD1.WidthRequest = Number1BorderD1.HeightRequest = numberBorderWidth;
        if (Number2BorderD2 != null) Number2BorderD2.WidthRequest = Number2BorderD2.HeightRequest = numberBorderWidth;
        if (Number3BorderD3 != null) Number3BorderD3.WidthRequest = Number3BorderD3.HeightRequest = numberBorderWidth;
        if (Number4BorderD4 != null) Number4BorderD4.WidthRequest = Number4BorderD4.HeightRequest = numberBorderWidth;

        if (SelectPaymentLabel != null) SelectPaymentLabel.FontSize = labelFontSize;
        if (SelectBetTypeLabel != null) SelectBetTypeLabel.FontSize = labelFontSize;
        if (SelectDrawLabel != null) SelectDrawLabel.FontSize = labelFontSize;
        if (SelectDateLabel != null) SelectDateLabel.FontSize = labelFontSize;
        if (MenuOptionsLabel != null) MenuOptionsLabel.FontSize = labelFontSize;
        if (ErrorLabel != null) ErrorLabel.FontSize = errorLabelFont;

        if (Number1Entry != null) Number1Entry.FontSize = pickerFontSize;
        if (Number2Entry != null) Number2Entry.FontSize = pickerFontSize;
        if (Number3Entry != null) Number3Entry.FontSize = pickerFontSize;

        if (Number1Entry4 != null) Number1Entry4.FontSize = pickerFontSize;
        if (Number2Entry4 != null) Number2Entry4.FontSize = pickerFontSize;
        if (Number3Entry4 != null) Number3Entry4.FontSize = pickerFontSize;
        if (Number4Entry4 != null) Number4Entry4.FontSize = pickerFontSize;

        if (PriceEntry != null) PriceEntry.FontSize = pickerFontSize;

        if (StraightButton != null) StraightButton.FontSize = buttonFontSize;
        if (BoxButton != null) BoxButton.FontSize = buttonFontSize;
        if (TwoWayButton != null) TwoWayButton.FontSize = buttonFontSize;
        if (WheelButton != null) WheelButton.FontSize = buttonFontSize;
        if (MiddayButton != null) MiddayButton.FontSize = buttonFontSize;
        if (EveningButton != null) EveningButton.FontSize = buttonFontSize;
        if (BothButton != null) BothButton.FontSize = buttonFontSize;

        if (BuyTicketsButton != null) BuyTicketsButton.FontSize = buttonFontSize;
        if (HistoryButton != null) HistoryButton.FontSize = buttonFontSize;
        if (WinningNumbersButton != null) WinningNumbersButton.FontSize = buttonFontSize;

        if (Days2 != null) Days2.FontSize = buttonFontSize;
        if (Days3 != null) Days3.FontSize = buttonFontSize;
        if (Days4 != null) Days4.FontSize = buttonFontSize;
        if (Days5 != null) Days5.FontSize = buttonFontSize;
        if (Days6 != null) Days6.FontSize = buttonFontSize;
        if (Days7 != null) Days7.FontSize = buttonFontSize;
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
            KeyboardHelper.HideKeyboard();
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
            KeyboardHelper.HideKeyboard();
        }
    }

    private void OnNumber3TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Number3Entry.Text))
        {
            Number3Entry.Unfocus();
            KeyboardHelper.HideKeyboard();
        }
    }

    private void OnNumber1TextChangedD(object sender, TextChangedEventArgs e)
    {
        var currentIsEmpty = string.IsNullOrEmpty(Number1Entry4.Text);
        var nextIsEmpty = string.IsNullOrEmpty(Number2Entry4.Text);
        if (!currentIsEmpty && nextIsEmpty)
        {
            Number2Entry4.Focus();
        }
        else if (!currentIsEmpty)
        {
            Number1Entry4.Unfocus();
            KeyboardHelper.HideKeyboard();
        }
    }

    private void OnNumber2TextChangedD(object sender, TextChangedEventArgs e)
    {
        var currentIsEmpty = string.IsNullOrEmpty(Number2Entry4.Text);
        var nextIsEmpty = string.IsNullOrEmpty(Number3Entry4.Text);
        if (!currentIsEmpty && nextIsEmpty)
        {
            Number3Entry4.Focus();
        }
        else if (!currentIsEmpty)
        {
            Number2Entry4.Unfocus();
            KeyboardHelper.HideKeyboard();
        }
    }

    private void OnNumber3TextChangedD(object sender, TextChangedEventArgs e)
    {
        var currentIsEmpty = string.IsNullOrEmpty(Number3Entry4.Text);
        var nextIsEmpty = string.IsNullOrEmpty(Number4Entry4.Text);
        if (!currentIsEmpty && nextIsEmpty)
        {
            Number4Entry4.Focus();
        }
        else if (!currentIsEmpty)
        {
            Number3Entry4.Unfocus();
            KeyboardHelper.HideKeyboard();
        }
    }

    private void OnNumber4TextChangedD(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Number4Entry4.Text))
        {
            Number4Entry4.Unfocus();
            KeyboardHelper.HideKeyboard();
        }
    }

    public void ClearShoppingCart()
    {
        ShoppingCart.Clear();
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
            PriceEntry.Text = result;
        }
    }

    private void OnNumberEntryFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = string.Empty;
            entry.Focus();
        }
    }

    private int _currentNumberOfDaysSelected = 1;

    private void OnPresetDateSelected(object sender, EventArgs e)
    {
        var senderButton = (Button)sender;
        var selectedColor = Globals.GetColor("Selected");
        if (Equals(senderButton.BackgroundColor, selectedColor))
        {
            _currentNumberOfDaysSelected = 1;
            senderButton.BackgroundColor = Globals.GetColor("Primary");
            return;
        }

        DeselectAllOtherPresetDates();
        if (int.TryParse(senderButton.Text[0].ToString(), out int number))
        {
            senderButton.BackgroundColor = selectedColor;
            _currentNumberOfDaysSelected = number;
        }
        else
        {
            throw new FormatException(
                "The button that is using this methods text is invalid. Needs to start with a number");
        }
    }

    private void DeselectAllOtherPresetDates()
    {
        Days2.BackgroundColor = Globals.GetColor("Primary");
        Days3.BackgroundColor = Globals.GetColor("Primary");
        Days4.BackgroundColor = Globals.GetColor("Primary");
        Days5.BackgroundColor = Globals.GetColor("Primary");
        Days6.BackgroundColor = Globals.GetColor("Primary");
        Days7.BackgroundColor = Globals.GetColor("Primary");
    }

    private void OnPriceSet(object sender, EventArgs e)
    {
        try
        {
            var roundedValue = RoundToNearestPoint05(double.Parse(PriceEntry.Text));
            if (roundedValue > PriceLimit) roundedValue = PriceLimit;
            PriceEntry.Text = roundedValue.ToString("0.00");
        }
        catch (FormatException ex)
        {
            PriceEntry.Text = "1.00";
        }
    }

    private double RoundToNearestPoint05(double value)
    {
        return Math.Round(value / 0.05) * 0.05;
    }
    
    private int GetDayValue()
    {
        var today = DateTime.Now.DayOfWeek;
        var currentDay = today == DayOfWeek.Sunday ? 6 : (int)today - 1;
        
        try
        {
            var michiganTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var michiganTime = TimeZoneInfo.ConvertTime(DateTime.Now, michiganTimeZone);
            var targetTime = michiganTime.Date.AddHours(19).AddMinutes(8);
            
            currentDay = michiganTime > targetTime ? currentDay + 1 : currentDay;
        }
        catch (TimeZoneNotFoundException)
        {
            // Fallback for iOS if timezone is not available
            // Use local time as fallback
            var localTime = DateTime.Now;
            var targetTime = localTime.Date.AddHours(19).AddMinutes(8);
            
            currentDay = localTime > targetTime ? currentDay + 1 : currentDay;
        }
        catch (Exception)
        {
            // Additional fallback - just use the current day without timezone conversion
            // This ensures the app doesn't crash on any timezone-related issues
        }
        
        return currentDay % 7;
    }

    private void MoveNumberOfDays()
    {
        try
        {
            var currentDayNumber = GetDayValue();

            // Check if all required buttons exist before proceeding
            if (Days2 == null || Days3 == null || Days4 == null || 
                Days5 == null || Days6 == null || Days7 == null)
            {
                return; // Exit early if buttons aren't initialized yet
            }

            var days = new List<Button>()
            {
                Days2, Days3, Days4, Days5, Days6, Days7
            };
            days.ForEach(d => d.IsVisible = true);

            switch (currentDayNumber)
            {
                case 6:
                    Days2.IsVisible = false;
                    goto case 5;
                case 5:
                    Days3.IsVisible = false;
                    goto case 4;
                case 4:
                    Days4.IsVisible = false;
                    goto case 3; 
                case 3:
                    Days5.IsVisible = false;
                    goto case 2;
                case 2:
                    Days6.IsVisible = false;
                    goto case 1;
                case 1:
                    Days7.IsVisible = false;
                    break;  
            }
        }
        catch (Exception ex)
        {
            // Log the error but don't crash the app
            System.Diagnostics.Debug.WriteLine($"Error in MoveNumberOfDays: {ex.Message}");
        }
    }
}