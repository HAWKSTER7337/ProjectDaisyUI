using Daily3_UI.Classes;
using Daily3_UI.Enums;

namespace Daily3_UI.Clients;

public static class BuyTicketClient
{
    /// <summary>
    ///     Client For Buying a Ticket
    /// </summary>
    public static async Task<string> BuyTicket(Ticket ticket)
    {
        var httpClient = new HttpClient();
        var baseUrl = "http://10.0.2.2:5198/";
        var endPoint = "api/BuyTicket";

        var apiUrl = ticket.ToApiUrl(baseUrl, endPoint);

        if (!isValidDate(ticket.TimeOfDay, ticket.Date))
        {
            Console.WriteLine("Not valid date to buy Ticket");
            return "Not Valid Date to Buy Ticket";
        }

        try
        {
            var response = await httpClient.GetAsync(apiUrl);

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
        return true;
    }
}