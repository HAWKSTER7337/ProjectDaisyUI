using System.Web;

namespace Daily3_UI.Clients;

public static class VerifyUserClient
{
    public static async Task<string?> VerifyUser(string username, string password)
    {
        var httpClient = new HttpClient();
        var endPoint = "api/VerifyUser";

        var uriBuilder = new UriBuilder(ClientSideData.BaseUrl + endPoint);
        var queryParameters = HttpUtility.ParseQueryString(string.Empty);
        queryParameters["username"] = username;
        queryParameters["password"] = password;
        uriBuilder.Query = queryParameters.ToString();
        var apiUrl = uriBuilder.ToString();
        Console.WriteLine(apiUrl);

        try
        {
            var response = await httpClient.GetAsync(apiUrl);

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