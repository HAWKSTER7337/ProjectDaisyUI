using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;
using Daily3_UI.Clients;

namespace Daily3_UI.Pages.PagesDaily3;

public partial class TicketPopupPage : Popup
{
    public ObservableCollection<Ticket> Tickets { get; }

    private readonly BuyTickets _page;

    private List<Ticket> TicketList
    {
        get
        {
            var ticketList = new List<Ticket>();
            foreach (var ticket in Tickets)
            {
                ticketList.Add(ticket);
            }

            return ticketList;
        }
    }

    public ICommand RemoveCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand BuyTicketsCommand { get;  }
    
    
    public TicketPopupPage(List<Ticket> tickets, BuyTickets page)
    {
        InitializeComponent();
        _page = page;

        Tickets = new ObservableCollection<Ticket>(tickets);
        RemoveCommand = new Command<Ticket>(ticket =>
        {
            Tickets.Remove(ticket);
            var index = tickets.IndexOf(ticket);
            if (index != -1) tickets.RemoveAt(index);
            if (Tickets.Count == 0) Close();
        });
        CloseCommand = new Command(CancelTicketPurchase);
        BuyTicketsCommand = new Command(BuyTickets);

        BindingContext = this;
    }

    private void CancelTicketPurchase()
    {
        _page.ClearShoppingCart();

        Close();
    }

    private async void BuyTickets()
    {
        string errorCode;
        if (TicketList.First() is Ticket3)
        {
            var ticket3List = TicketList.Select(ticket => (Ticket3)ticket).ToList();
            errorCode = await BuyTicketClient.BuyTicketsDaily3(ticket3List);
        }
        else
        {
            var ticket4List = TicketList.Select(ticket => (Ticket4)ticket).ToList();
            errorCode = await BuyTicketClient.BuyTicketsDaily4(ticket4List);
        }
        _page.ChangeErrorLabelColor( errorCode != "Tickets sent successfully"
             ? Globals.GetColor("DailyRed")
             : Globals.GetColor("SuccessGreen"));
        _page.SetErrorLabel(errorCode);
        _page.ClearShoppingCart();
        Close();
    }
}