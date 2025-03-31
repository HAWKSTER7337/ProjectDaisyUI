using System.Text.Json;
using System.Web;
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
        var uriBuilder = new UriBuilder(ClientSideData.BaseUrl + endpoint);
        var queryParameters = HttpUtility.ParseQueryString(string.Empty);
        queryParameters["userId"] = Globals.UserId.ToString();
        uriBuilder.Query = queryParameters.ToString();
        var uriString = uriBuilder.ToString();
        var client = new HttpClient();


        try
        {
            var response = await client.GetAsync(uriString);

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

        return new List<T>();
    }
}