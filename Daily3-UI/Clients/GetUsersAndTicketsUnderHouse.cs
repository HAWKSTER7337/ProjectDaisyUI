using System.Text;
using System.Text.Json;
using Daily3_UI.Classes;

namespace Daily3_UI.Clients;

public static class GetUsersAndTicketsUnderHouse
{
    public static async Task<List<User>> GetUsersDaily()
    {
        var apiUrl = $"{ClientSideData.BaseUrl}api/TicketHistory/house-tickets";
        var jsonResponse = await GetUsers(apiUrl);
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, TicketAndWeeklyTotal>>(jsonResponse);

        List<User> listOfUsers = new();
        foreach (var userPair in dictionary)
        {
            var user = new User(userPair.Key, userPair.Value.WinningTotal);
            user.Tickets.AddRange(userPair.Value.Tickets());
            listOfUsers.Add(user);
        }

        return listOfUsers;
    }

    private static async Task<string> GetUsers(string apiUrl)
    {
        var httpClient = new HttpClient();
        var guid = Globals.UserId;

        try
        {
            var jsonString = JsonSerializer.Serialize(guid);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);

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

    public static async Task<List<User>> GetUnverifiedUsers()
    {
        try
        {
            var httpClient = new HttpClient();
            var apiUrl = $"{ClientSideData.BaseUrl}api/HouseUnverifiedUsers/unverified";
            var jsonString = JsonSerializer.Serialize(Globals.UserId);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                var usersDictionary = JsonSerializer.Deserialize<Dictionary<string, Guid>>(responseBody);
                return usersDictionary.Keys.Select(username => new User(username, usersDictionary[username])).ToList();
            }

            Console.WriteLine("Servers could be down!");
            return new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }

    public static async Task<bool> AddUserUnderHouse(Guid userBeingAddedUnderHouse)
    {
        try
        {
            var httpClient = new HttpClient();
            var apiUrl = $"{ClientSideData.BaseUrl}api/HouseUnverifiedUsers/approve";

            var jsonString = JsonSerializer.Serialize(
                new ApproveUserRequest
                {
                    UserId = Globals.UserId,
                    UnverifiedUserId = userBeingAddedUnderHouse
                });
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                var usersDictionary = JsonSerializer.Deserialize<string>(responseBody);
                return true;
            }

            Console.WriteLine("Servers could be down!");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }

    public class ApproveUserRequest
    {
        public Guid? UserId { get; set; }
        public Guid UnverifiedUserId { get; set; }
    }
}