using System.Text;
using System.Text.Json;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class TicketHistoryClient
{
    public static async Task<List<Ticket4>> GetTicketHistoryDaily4()
    {
        var endpoint = "api/TicketHistory/Daily4";
        return await GetTicketHistory<Ticket4>(endpoint);
    }

    public static async Task<List<Ticket3>> GetTicketHistoryDaily3()
    {
        var endpoint = "api/TicketHistory";
        return await GetTicketHistory<Ticket3>(endpoint);
    }

    private static async Task<List<T>> GetTicketHistory<T>(string endpoint)
        where T : Ticket
    {
        var jsonString = JsonSerializer.Serialize(Globals.UserId);
        var client = new HttpClient();
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var uriString = $"{ClientSideData.BaseUrl}{endpoint}";

        try
        {
            var response = await client.PostAsync(uriString, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var formattedResponse = JsonSerializer.Deserialize<List<T>>(responseContent);
                return formattedResponse;
            }

            Console.WriteLine($"API call failed Status code: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling the API: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling the API: {ex.Message}");
        }

        return new List<T>();
    }
}