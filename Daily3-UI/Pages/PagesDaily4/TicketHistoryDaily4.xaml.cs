using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages;

public partial class TicketHistoryDaily4 : ContentPage
{
    public TicketHistoryDaily4()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        TicketLoaderIsBusy = true;
        Title.Text = await GetTitleString();
        SearchToggle.IsToggled = false;
        _userTickets = await TicketHistoryClient.GetTicketHistoryDaily4();
        BindingContext = new HistoryPageViewModel<Ticket4>(_userTickets);
        TicketLoaderIsBusy = false;
    }

    private static async Task<string> GetTitleString()
    {
        var winningTotal = await TicketHistoryClient.GetWeeklyTotal();
        return $"Ticket History | Weekly Total: ${winningTotal:F2}";
    }


    /// <summary>
    ///     Displays the ticket history on the page. If not
    ///     will show a loading bar when the ticket loader is busy
    /// </summary>
    private bool _ticketLoaderIsBusy;

    public bool TicketLoaderIsBusy
    {
        get => _ticketLoaderIsBusy;
        private set
        {
            _ticketLoaderIsBusy = value;
            if (_ticketLoaderIsBusy)
            {
                TicketCollectionView.IsVisible = false;
                TicketLoader.IsRunning = true;
                TicketLoader.IsVisible = true;
            }
            else
            {
                TicketLoader.IsVisible = false;
                TicketLoader.IsRunning = false;
                TicketCollectionView.IsVisible = true;
            }

            OnPropertyChanged(nameof(TicketCollectionView));
        }
    }

    /// <summary>
    ///     All the users Tickets
    /// </summary>
    private List<Ticket4> _userTickets;

    /// <summary>
    ///     Checking if the tickets should be filtered by date
    /// </summary>
    private bool _shouldFilterByDate;

    private bool ShouldNotFilterByDate => !_shouldFilterByDate;

    private DateTime _currentSetDate = DateTime.Today;

    /// <summary>
    ///     Toggle switch handler
    /// </summary>
    private void ToggleDateSwitch(object sender, ToggledEventArgs e)
    {
        _shouldFilterByDate = e.Value;
        FilterByDate(_currentSetDate);
        OnPropertyChanged(nameof(TicketCollectionView));
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
    ///     The filter handling
    /// </summary>
    private void FilterByDate(DateTime date)
    {
        if (ShouldNotFilterByDate)
        {
            BindingContext = new HistoryPageViewModel<Ticket4>(_userTickets);
            OnPropertyChanged(nameof(TicketCollectionView));
            return;
        }

        var filteredDates = _userTickets.Where(ticket => DateTime.Parse(ticket.Date).Date == date.Date).ToList();
        BindingContext = new HistoryPageViewModel<Ticket4>(filteredDates);
        OnPropertyChanged(nameof(TicketCollectionView));
    }

    /// <summary>
    ///     Used to scale the page with the users phone
    ///     for the sake of consistency
    /// </summary>
    private void OnSizeChanged(object sender, EventArgs e)
    {
        var screenWidth = Width;
        var screenHeight = Height;

        // Scale values for elements
        var borderPadding = screenWidth * 0.05;
        var labelFontSize = screenWidth * 0.03;
        var titleFontSize = screenWidth * 0.04;
        var pickerFontSize = screenWidth * 0.04;

        // Adjust elements accordingly
        BorderTitle.Padding = new Thickness(borderPadding);
        Title.FontSize = titleFontSize;

        DateSearchLabel.FontSize = labelFontSize;

        DatePicker.FontSize = pickerFontSize;
        SearchToggleLabel.FontSize = labelFontSize;
    }
}