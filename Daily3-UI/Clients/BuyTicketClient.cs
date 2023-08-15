using System.Web;
using Daily3_UI.Enums;

namespace Daily3_UI.Clients;

public static class BuyTicketClient
{
    public static async Task BuyTicket
    (
        int number1,
        int number2,
        int number3,
        string price,
        TicketType ticketType,
        TOD timeOfDay,
        string date
    )
    {
        var httpClient = new HttpClient();
        var baseUrl = "http://10.0.2.2:5198/";
        var endPoint = "api/BuyTicket";

        var uriBuilder = new UriBuilder(baseUrl + endPoint);
        var queryParameters = HttpUtility.ParseQueryString(string.Empty);
        queryParameters["number1"] = number1.ToString();
        queryParameters["number2"] = number2.ToString();
        queryParameters["number3"] = number3.ToString();
        queryParameters["price"] = price;
        queryParameters["ticketType"] = ((int)ticketType).ToString();
        queryParameters["timeOfDay"] = ((int)timeOfDay).ToString();
        queryParameters["date"] = date;
        uriBuilder.Query = queryParameters.ToString();
        var apiUrl = uriBuilder.ToString();

        try
        {
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                return;
            }

            Console.WriteLine("Request failed with status code: " + response.StatusCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}