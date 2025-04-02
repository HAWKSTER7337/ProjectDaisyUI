using System.Text;
using System.Text.Json;
using Daily3_UI.Clients.ClientRequests;

namespace Daily3_UI.Clients;

public static class VerifyUserClient
{
    public static async Task<string?> VerifyUser(string username, string password)
    {
        var credentials = new Credentials
        {
            Username = username,
            Password = password
        };

        var httpClient = new HttpClient();
        var endPoint = "api/VerifyUser";

        var jsonString = JsonSerializer.Serialize(credentials);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.PostAsync(ClientSideData.BaseUrl + endPoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                if (responseBody == "User has not been accepted") return "Still waiting to be accepted.";
                return responseBody == "False" ? "Invalid Username or password" : responseBody;
            }

            Console.WriteLine("Request failed with status code: " + response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }
}