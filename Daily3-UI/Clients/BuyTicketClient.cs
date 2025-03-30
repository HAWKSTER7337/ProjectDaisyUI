using System.Text;
using Daily3_UI.Classes;
using Daily3_UI.Enums;

namespace Daily3_UI.Clients;

public static class BuyTicketClient
{
    /// <summary>
    ///     Client For Buying a Ticket
    /// </summary>
    public static async Task<string> BuyTicketDaily3(Ticket3 ticket)
    {
        var endPoint = "api/BuyTicket";
        return await BuyTicket(ticket, endPoint);
    }

    public static async Task<string> BuyTicketDaily4(Ticket4 ticket)
    {
        var endPoint = "api/BuyTicket/Daily4";
        return await BuyTicket(ticket, endPoint);
    }

    private static async Task<string> BuyTicket<T>(T ticket, string apiEndpoint)
        where T : Ticket
    {
        var httpClient = new HttpClient();

        var apiUrl = ticket.ToApiUrl(ClientSideData.BaseUrl, apiEndpoint);

        if (!isValidDate(ticket.TimeOfDay, ticket.Date))
        {
            Console.WriteLine("Not valid date to buy Ticket");
            return "Not Valid Date to Buy Ticket";
        }

        try
        {
            var stringContent = new StringContent(apiUrl, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                return "";
            }

            Console.WriteLine("Request failed with status code: " + response.StatusCode);
            return "Request failed";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Shi....ooot something went wrong!";
        }
    }

    /// <summary>
    ///     Validating that the date given is still valid to buy Tickets for
    /// </summary>
    /// <param name="timeOfDay">The Time of day that the ticket is being bought for</param>
    /// <param name="date">The date that the ticket is designed for</param>
    private static bool isValidDate(TOD? timeOfDay, string date)
    {
        // todo make this work and actually pick a real time
        // NOTE this is currently working on the other side. If you request to buy a ticket at a non valid time the ticket 
        // will not be bought and you will get an error code on this side of the app how ever a double check would not be a bad idea at all
        return true;
    }
}