using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class TicketHistory : ContentPage
{
    public TicketHistory()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        SearchToggle.IsToggled = false;
        _userTickets = await TicketHistoryClient.GetTicketHistory();
        BindingContext = new HistoryPageViewModel(_userTickets);
    }

    /// <summary>
    ///     All of the users Tickets
    /// </summary>
    private List<Ticket> _userTickets;

    /// <summary>
    ///     Checking if the tickets should be filtered by date
    /// </summary>
    private bool _shouldFilterByDate;

    private DateTime _currentSetDate = DateTime.Today;

    /// <summary>
    ///     Toggle switch handler
    /// </summary>
    private void ToggleDateSwitch(object sender, ToggledEventArgs e)
    {
        _shouldFilterByDate = e.Value;
        FilterByDate(_currentSetDate);
    }

    /// <summary>
    ///     What happens when you change the date
    /// </summary>
    private void DateChanged(object sender, DateChangedEventArgs e)
    {
        _currentSetDate = e.NewDate;
        FilterByDate(_currentSetDate);
    }

    /// <summary>
    ///     Actual filter
    /// </summary>
    private void FilterByDate(DateTime date)
    {
        if (!_shouldFilterByDate)
        {
            BindingContext = new HistoryPageViewModel(_userTickets);
            return;
        }

        var filteredDates = _userTickets.Where(ticket => DateTime.Parse(ticket.Date).Date == date.Date).ToList();
        BindingContext = new HistoryPageViewModel(filteredDates);
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        var screenWidth = Width;
        var screenHeight = Height;

        // Scale values for elements
        var borderPadding = screenWidth * 0.05;
        var labelFontSize = screenWidth * 0.03;
        var titleFontSize = screenWidth * 0.05;
        var pickerFontSize = screenWidth * 0.04;

        // Adjust elements accordingly
        BorderTitle.Padding = new Thickness(borderPadding);
        Title.FontSize = titleFontSize;

        DateSearchLabel.FontSize = labelFontSize;

        DatePicker.FontSize = pickerFontSize;
        SearchToggleLabel.FontSize = labelFontSize;
    }
}

public class HistoryPageViewModel
{
    public List<Ticket> Tickets { get; set; }

    public HistoryPageViewModel(List<Ticket> tickets)
    {
        Tickets = tickets;
    }
}