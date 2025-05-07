using System.Text.Json.Serialization;

namespace Daily3_UI.Classes;

public class TicketAndWeeklyTotal
{
    [JsonPropertyName("Tickets3")] public List<Ticket3> Tickets3 { get; set; }

    [JsonPropertyName("Tickets4")] public List<Ticket4> Tickets4 { get; set; }
    public double WinningTotal { get; set; }

    public List<Ticket> Tickets()
    {
        var tickets = new List<Ticket>();
        tickets.AddRange(Tickets3);
        tickets.AddRange(Tickets4);
        return tickets;
    }
}