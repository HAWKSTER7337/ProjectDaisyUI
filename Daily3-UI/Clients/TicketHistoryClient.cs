using System.Text;
using System.Text.Json;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class TicketHistoryClient
{
    public static async Task<List<Ticket>> GetTicketHistory()
    {
        var endpoint = "api/TicketHistory";
        var jsonString = JsonSerializer.Serialize((Guid)Globals.UserId);
        var client = new HttpClient();
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var uriString = $"{ClientSideData.BaseUrl}{endpoint}";

        try
        {
            var response = await client.PostAsync(uriString, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var formattedResponse =
                    JsonSerializer.Deserialize<KeyValuePair<List<Ticket3>, List<Ticket4>>>(responseContent);
                return MergeTickets(formattedResponse);
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error calling the api: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error calling the api: {e.Message}");
        }

        return new List<Ticket>();
    }

    private static List<Ticket> MergeTickets(KeyValuePair<List<Ticket3>, List<Ticket4>> ticketGroups)
    {
        List<Ticket> tickets = new();
        
        ticketGroups.Key.ForEach(ticket => tickets.Add(ticket));
        ticketGroups.Value.ForEach(ticket => tickets.Add(ticket));
        return tickets.OrderBy(ticket => DateTime.Parse(ticket.Date)).ToList();
    }

    public static async Task<double> GetWeeklyTotal()
    {
        return await GetWeeklyTotalWithGuid((Guid)Globals.UserId);
    }

    public static async Task<double> GetWeeklyTotalWithGuid(Guid userId)
    {
        var endpoint = "api/TicketHistory/weekly-total";
        var jsonString = JsonSerializer.Serialize(userId);
        var client = new HttpClient();
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var uriString = $"{ClientSideData.BaseUrl}{endpoint}";

        try
        {
            var response = await client.PostAsync(uriString, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var formattedResponse = JsonSerializer.Deserialize<double>(responseContent);
                return formattedResponse;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling the API: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling the API: {ex.Message}");
        }

        return 0.0;
    }
}