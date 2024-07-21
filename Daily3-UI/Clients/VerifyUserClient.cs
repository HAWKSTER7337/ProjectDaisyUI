using System.Web;

namespace Daily3_UI.Clients;

public static class VerifyUserClient
{
    public static async Task<Guid?> VerifyUser(string username, string password)
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
                return responseBody == "False" ? null : Guid.Parse(responseBody);
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