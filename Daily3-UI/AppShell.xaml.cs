using Daily3_UI.Classes;
using Daily3_UI.Enums;
using Daily3_UI.Pages;

namespace Daily3_UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var isAdmin =
            !(Globals.Status is null) && // Checking if the value is defined
            Globals.Status >= Status.House; // Checking if they are the proper Status
        if (!isAdmin) return;

        var shellContent = new ShellContent
        {
            Title = "Entrants Tickets",
            ContentTemplate = new DataTemplate(typeof(TicketHistory)),
            Route = "TicketHistory"
        };

        TabBar.Items.Add(shellContent);
    }
}