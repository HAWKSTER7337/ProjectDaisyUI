namespace Daily3_UI.Clients;

public static class CreateAccountClient
{
    public static async Task<string> CreateAccount(string username, string password)
    {
        var httpClient = new HttpClient();

        var apiUrl = $"{ClientSideData.BaseUrl}api/CreateUser?username={username}&password={password}";

        try
        {
            var response = await httpClient.PostAsync(apiUrl, null);

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