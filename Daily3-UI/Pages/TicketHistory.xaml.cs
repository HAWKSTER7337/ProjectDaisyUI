using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.Enums;

namespace Daily3_UI.Pages;

public partial class TicketHistory : ContentPage
{
    public TicketHistory()
    {
        InitializeComponent();
        _viewModel = new HistoryPageViewModel<Ticket>(new ObservableCollection<Ticket>());
        BindingContext = _viewModel;
    }

    private readonly HistoryPageViewModel<Ticket> _viewModel;

    protected override async void OnAppearing()
    {
        TicketLoaderIsBusy = true;
        Title.Text = await GetTitleString();
        SearchToggle.IsToggled = false;

        var allTickets = await TicketHistoryClient.GetTicketHistoryDaily3();
        _userTickets = new ObservableCollection<Ticket3>(allTickets);

        _viewModel.Tickets.Clear();
        foreach (var ticket in _userTickets)
            _viewModel.Tickets.Add(ticket);

        TicketLoaderIsBusy = false;
    }

    private async void BackToBuyTicketsPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
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
    private ObservableCollection<Ticket3> _userTickets;

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
        _viewModel.Tickets.Clear();

        if (ShouldNotFilterByDate)
        {
            foreach (var ticket in _userTickets)
                _viewModel.Tickets.Add(ticket);
        }
        else
        {
            var filtered = _userTickets.Where(t => DateTime.Parse(t.Date).Date == date.Date);
            foreach (var ticket in filtered)
                _viewModel.Tickets.Add(ticket);
        }
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
        var titleFontSize = screenWidth * 0.03;
        var pickerFontSize = screenWidth * 0.04;

        // Adjust elements accordingly
        BorderTitle.Padding = new Thickness(borderPadding);
        Title.FontSize = titleFontSize;

        DateSearchLabel.FontSize = labelFontSize;

        DatePicker.FontSize = pickerFontSize;
        SearchToggleLabel.FontSize = labelFontSize;
    }
}

public class HistoryPageViewModel<T>
    where T : Ticket
{
    public ObservableCollection<T> Tickets { get; set; }

    public HistoryPageViewModel(ObservableCollection<T> tickets)
    {
        Tickets = tickets;
    }
}

/// <summary>
///     Class used to transfer the WinningStatus Enum into a number to
///     display the numbers on the screen
/// </summary>
public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is WinningStatus status))
        {
            Debug.WriteLine("Value must be a winning status");
            return Colors.Black;
        }

        return status switch
        {
            WinningStatus.Loser => Globals.GetColor("DailyRed"),
            WinningStatus.Winner => Globals.GetColor("SuccessGreen"),
            _ => Globals.GetColor("White")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}