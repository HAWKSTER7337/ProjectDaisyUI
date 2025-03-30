using System.Text.Json;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class GetUsersAndTicketsUnderHouse
{
    public static async Task<List<User>> getUsersDaily3()
    {
        var guid = Globals.UserId.ToString();
        var apiUrl = $"{ClientSideData.BaseUrl}api/TicketHistory/house-tickets?userId={guid}";
        var jsonResponse = await GetUsers(apiUrl);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<Ticket3>>>(jsonResponse);

        List<User> listOfUsers = new();
        foreach (var userPair in dictionary)
        {
            var user = new User(userPair.Key);
            user.Tickets3.AddRange(userPair.Value);
            listOfUsers.Add(user);
        }

        return listOfUsers;
    }

    public static async Task<List<User>> getUsersDaily4()
    {
        var guid = Globals.UserId.ToString();
        var apiUrl = $"{ClientSideData.BaseUrl}api/TicketHistory/daily4/house-tickets?userId={guid}";
        var jsonResponse = await GetUsers(apiUrl);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<Ticket4>>>(jsonResponse);

        List<User> listOfUsers = new();
        foreach (var userPair in dictionary)
        {
            var user = new User(userPair.Key);
            user.Tickets4.AddRange(userPair.Value);
            listOfUsers.Add(user);
        }

        return listOfUsers;
    }

    private static async Task<string> GetUsers(string apiUrl)
    {
        var httpClient = new HttpClient();

        try
        {
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                return responseBody;
            }

            Console.WriteLine("Servers could be down!");
            return "Server Error reach out to moderators for help. The servers could be under maintenance";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }
}