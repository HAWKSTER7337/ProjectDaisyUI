using Daily3_UI.Classes;
using Daily3_UI.Enums;
using Daily3_UI.Pages;

namespace Daily3_UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes
        Routing.RegisterRoute("Home3", typeof(WinningNumbersPage));
        Routing.RegisterRoute("Buy3", typeof(BuyTickets));
        Routing.RegisterRoute("History3", typeof(TicketHistory));
        Routing.RegisterRoute("Entrants3", typeof(HousePage));
        
        Routing.RegisterRoute("Home4", typeof(WinningNumbersPageDaily4));
        Routing.RegisterRoute("Buy4", typeof(BuyTicketsDaily4));
        Routing.RegisterRoute("History4", typeof(TicketHistoryDaily4));
        Routing.RegisterRoute("Entrants4", typeof(HousePageDaily4));
   
        Routing.RegisterRoute("LoginPage", typeof(LogInPage));
        Routing.RegisterRoute("ChangePassword", typeof(ChangePasswordPage));
        Routing.RegisterRoute("CreateAccount", typeof(CreateAccountPage));
    }
}