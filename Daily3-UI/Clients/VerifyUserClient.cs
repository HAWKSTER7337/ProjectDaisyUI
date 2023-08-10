using System.Web;

namespace Daily3_UI.Clients;

public static class VerifyUserClient
{
    public static async Task<int?> VerifyUser(string username, string password)
    {
        var httpClient = new HttpClient();
        var baseUrl = "http://10.0.2.2:5198/";
        var endPoint = "api/VerifyUser";

        var uriBuilder = new UriBuilder(baseUrl + endPoint);
        var queryParameters = HttpUtility.ParseQueryString(string.Empty);
        queryParameters["username"] = username;
        queryParameters["password"] = password;
        uriBuilder.Query = queryParameters.ToString();
        var apiUrl = uriBuilder.ToString();

        try
        {
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                return responseBody == "False" ? null : int.Parse(responseBody);
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