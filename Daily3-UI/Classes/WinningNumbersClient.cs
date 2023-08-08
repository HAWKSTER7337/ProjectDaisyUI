using System.Text.Json;

namespace Daily3_UI.Classes;

public static class WinningNumbersClient
{
    public static async Task<List<WinningNumber>> GetWinningNumbers()
    {
        var apiUrl = "http://10.0.2.2:5198/api/WinningNumbers";
        var client = new HttpClient();

        try
        {
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var formattedResponse = JsonSerializer.Deserialize<List<WinningNumber>>(responseContent);
                return formattedResponse;
            }

            Console.WriteLine("API call failed. Status code: " + response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Error calling the API: " + ex.Message);
        }

        return new List<WinningNumber>();
    }
}