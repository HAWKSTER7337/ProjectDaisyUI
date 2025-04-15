namespace Daily3_UI.Classes;

public class TicketAndWeeklyTotal<T>
    where T : Ticket
{
    public List<T> Tickets { get; set; }
    public double WinningTotal { get; set; }
}