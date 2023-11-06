using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Daily3_UI.Classes;

namespace Daily3_UI.Pages;

public partial class TicketPopupPage : Popup
{
    public ObservableCollection<Ticket> Tickets { get; }
    public ICommand RemoveCommand { get; }
    public ICommand CloseCommand { get; }

    public TicketPopupPage(List<Ticket> tickets)
    {
        InitializeComponent();

        Tickets = new ObservableCollection<Ticket>(tickets);
        RemoveCommand = new Command<Ticket>(ticket =>
        {
            Tickets.Remove(ticket);
            var index = tickets.IndexOf(ticket);
            if (index != -1) tickets.RemoveAt(index);
        });
        CloseCommand = new Command(ClosePopup);

        BindingContext = this;
    }

    private void ClosePopup()
    {
        // Close the popup
        Close();
    }
}