using System.Text.Json;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class WinningNumbersClient
{
    public static async Task<List<WinningNumberDaily3>> GetWinningNumbersDaily3()
    {
        var apiUrl = ClientSideData.BaseUrl + "api/WinningNumbers";
        return await GetAllWinningNumbers<WinningNumberDaily3>(apiUrl);
    }

    public static async Task<List<WinningNumberDaily4>> GetWinningNumbersDaily4()
    {
        var apiUrl = ClientSideData.BaseUrl + "api/WinningNumbers/Daily4";
        return await GetAllWinningNumbers<WinningNumberDaily4>(apiUrl);
    }

    private static async Task<List<T>> GetAllWinningNumbers<T>(string apiUrl)
        where T : WinningNumberDaily3
    {
        var client = new HttpClient();

        try
        {
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var formattedResponse = JsonSerializer.Deserialize<List<T>>(responseContent);
                return formattedResponse;
            }

            Console.WriteLine("API call failed. Status code: " + response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Error calling the API: " + ex.Message);
        }

        return new List<T>();
    }
}