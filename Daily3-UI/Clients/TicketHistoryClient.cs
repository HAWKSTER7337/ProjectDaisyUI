using System.Text.Json;
using System.Web;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class TicketHistoryClient
{
    public static async Task<List<Ticket>> GetTicketHistory()
    {
        var baseUrl = "http://10.0.2.2:8080/";
        var endpoint = "api/TicketHistory";
        var uriBuilder = new UriBuilder(baseUrl + endpoint);
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
                var formattedResponse = JsonSerializer.Deserialize<List<Ticket>>(responseContent);
                return formattedResponse;
            }

            Console.WriteLine($"API call failed Status code: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling the API: {ex.Message}");
        }

        return new List<Ticket>();
    }
}