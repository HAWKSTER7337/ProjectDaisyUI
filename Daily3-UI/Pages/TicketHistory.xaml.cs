using Daily3_UI.Classes;
using Daily3_UI.Enums;

namespace Daily3_UI.Pages;

public partial class TicketHistory : ContentPage
{
    public TicketHistory()
    {
        InitializeComponent();
        BindingContext = new HistoryPageViewModel(_userTickets);
    }

    /// <summary>
    ///     All of the users Tickets
    /// </summary>
    private readonly List<Ticket> _userTickets = new()
    {
        new Ticket
        {
            Number1 = "1", Number2 = "2", Number3 = "3", Date = "2023-08-24", Price = "0.25", Type = TicketType.Box,
            TimeOfDay = TOD.Evening
        },
        new Ticket
        {
            Number1 = "1", Number2 = "2", Number3 = "3", Date = "2023-09-24", Price = "1.00",
            Type = TicketType.Straight, TimeOfDay = TOD.Evening
        }
    };

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

        var test = DateTime.Parse(_userTickets[0].Date).Date;
        var simpletest = test == date.Date;
        var filteredDates = _userTickets.Where(ticket => DateTime.Parse(ticket.Date).Date == date.Date).ToList();
        BindingContext = new HistoryPageViewModel(filteredDates);
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