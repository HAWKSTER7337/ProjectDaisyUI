using System.Text;
using System.Text.Json;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class BuyTicketClient
{
    public static async Task<string> BuyTicketsDaily3(List<Ticket3> tickets)
    {
        var endPoint = "api/BuyTicket/bulk";
        return await BuyTicket(tickets, endPoint);
    }

    public static async Task<string> BuyTicketsDaily4(List<Ticket4> tickets)
    {
        var endPoint = "api/BuyTicket/bulk/Daily4";
        return await BuyTicket(tickets, endPoint);
    }

    private static async Task<string> BuyTicket<T>(List<T> tickets, string apiEndpoint)
        where T : Ticket
    {
        tickets.ForEach(ticket => ticket.UserId = Globals.UserId);

        var jsonString = JsonSerializer.Serialize(tickets);

        var httpClient = new HttpClient();
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(ClientSideData.BaseUrl + apiEndpoint, content);

        if (response.IsSuccessStatusCode)
        {
            return "Tickets sent successfully";
        }
        else
        {
            return response.StatusCode.ToString();
        }
    }
}