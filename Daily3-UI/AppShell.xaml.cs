using Daily3_UI.Classes;
using Daily3_UI.Enums;
using Daily3_UI.Pages;
using Daily3_UI.Pages.PagesDaily3;

namespace Daily3_UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes
        Routing.RegisterRoute("WinningNumbers3", typeof(WinningNumbersPage));
        Routing.RegisterRoute("History3", typeof(TicketHistory));
        Routing.RegisterRoute("Entrants3", typeof(HousePage));
        
        Routing.RegisterRoute("WinningNumbers4", typeof(WinningNumbersPageDaily4));
        Routing.RegisterRoute("History4", typeof(TicketHistoryDaily4));
        Routing.RegisterRoute("Entrants4", typeof(HousePageDaily4));
        
        Routing.RegisterRoute("ChangePassword", typeof(ChangePasswordPage));
        Routing.RegisterRoute("CreateAccount", typeof(CreateAccountPage));
    }
} 