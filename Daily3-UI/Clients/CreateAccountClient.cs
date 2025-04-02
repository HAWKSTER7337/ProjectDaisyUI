using System.Text;
using System.Text.Json;
using Daily3_UI.Clients.ClientRequests;

namespace Daily3_UI.Clients;

public static class CreateAccountClient
{
    public static async Task<string> CreateAccount(string username, string password)
    {
        var credentials = new Credentials
        {
            Username = username,
            Password = password
        };

        var httpClient = new HttpClient();
        var apiUrl = $"{ClientSideData.BaseUrl}api/CreateUser";
        var jsonString = JsonSerializer.Serialize(credentials);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                return responseBody;
            }

            Console.WriteLine("Servers could be down!");
            return "Server Error reach out to moderators for help. The servers may be under maintenance";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }
}