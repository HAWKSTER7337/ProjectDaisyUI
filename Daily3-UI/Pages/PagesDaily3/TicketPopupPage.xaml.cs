using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Clients;
using Daily3_UI.Pages.PagesDaily4;

namespace Daily3_UI.Pages.PagesDaily3;

public partial class TicketPopupPage : Popup
{
    public ObservableCollection<Ticket> Tickets { get; }

    private readonly BuyTickets _daily3Page;
    
    private readonly BuyTicketsDaily4 _daily4Page;

    private List<Ticket3> Tickets3
    {
        get
        {
            var ticketList = new List<Ticket3>();
            foreach (var ticket in Tickets)
            {
                var ticket3 = (Ticket3)ticket;
                ticketList.Add(ticket3);
            }

            return ticketList;
        }
    }
    
    private List<Ticket4> Tickets4
    {
        get
        {
            var ticketList = new List<Ticket4>();
            foreach (var ticket in Tickets)
            {
                var ticket4 = (Ticket4)ticket;
                ticketList.Add(ticket4);
            }

            return ticketList;
        }
    }

    public ICommand RemoveCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand BuyTicketsCommand { get;  }

    public TicketPopupPage(List<Ticket3> tickets, BuyTickets page)
    {
        InitializeComponent();
        _daily3Page = page;

        Tickets = new ObservableCollection<Ticket>(tickets);
        RemoveCommand = new Command<Ticket3>(ticket =>
        {
            Tickets.Remove(ticket);
            var index = tickets.IndexOf(ticket);
            if (index != -1) tickets.RemoveAt(index);
        });
        CloseCommand = new Command(CancelTicketPurchase);
        BuyTicketsCommand = new Command(BuyTickets3);

        BindingContext = this;
    }
    
    public TicketPopupPage(List<Ticket4> tickets, BuyTicketsDaily4 page)
    {
        InitializeComponent();
        _daily4Page = page;

        Tickets = new ObservableCollection<Ticket>(tickets);
        RemoveCommand = new Command<Ticket4>(ticket =>
        {
            Tickets.Remove(ticket);
            var index = tickets.IndexOf(ticket);
            if (index != -1) tickets.RemoveAt(index);
            if (Tickets.Count == 0) Close();
        });
        CloseCommand = new Command(CancelTicketPurchase);
        BuyTicketsCommand = new Command(BuyTickets4);

        BindingContext = this;
    }

    private void CancelTicketPurchase()
    {
        try
        {
            _daily3Page.ClearShoppingCart();
        }
        catch (Exception ex)
        {
            _daily4Page.ClearShoppingCart();
        }

        Close();
    }

    private async void BuyTickets4()
    {
        var errorCode = await BuyTicketClient.BuyTicketsDaily4(Tickets4);
        _daily4Page.ChangeErrorLabelColor( errorCode != "Tickets sent successfully"
            ? Globals.GetColor("DailyRed")
            : Globals.GetColor("SuccessGreen"));
        _daily4Page.SetErrorLabel(errorCode);
        _daily4Page.ClearShoppingCart();
        Close();
    }

    private async void BuyTickets3()
    {
        var errorCode = await BuyTicketClient.BuyTicketsDaily3(Tickets3);
        _daily3Page.ChangeErrorLabelColor( errorCode != "Tickets sent successfully"
             ? Globals.GetColor("DailyRed")
             : Globals.GetColor("SuccessGreen"));
        _daily3Page.SetErrorLabel(errorCode);
        _daily3Page.ClearShoppingCart();
        Close();
    }
}