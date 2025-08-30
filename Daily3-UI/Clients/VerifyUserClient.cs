using System.Text;
using System.Text.Json;
using Daily3_UI.Clients.ClientRequests;

namespace Daily3_UI.Clients;

public static class VerifyUserClient
{
    public static async Task<LoginResult> VerifyUser(string username, string password)
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
                
                // Handle specific response cases
                if (responseBody == "User has not been accepted")
                {
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorType = LoginErrorType.AccountPending,
                        Message = "Your account is still pending approval. Please wait for an administrator to accept your registration.",
                        UserGuid = null,
                        UserStatus = null
                    };
                }
                
                if (responseBody == "False")
                {
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorType = LoginErrorType.InvalidCredentials,
                        Message = "Invalid username or password. Please check your credentials and try again.",
                        UserGuid = null,
                        UserStatus = null
                    };
                }

                // Success case - parse the response
                try
                {
                    var guidAndStatusList = responseBody.Split(',');
                    if (guidAndStatusList.Length == 2 && 
                        Guid.TryParse(guidAndStatusList[0], out var userGuid) &&
                        int.TryParse(guidAndStatusList[1], out var status))
                    {
                        return new LoginResult
                        {
                            IsSuccess = true,
                            ErrorType = LoginErrorType.None,
                            Message = "Login successful!",
                            UserGuid = userGuid,
                            UserStatus = status
                        };
                    }
                    else
                    {
                        return new LoginResult
                        {
                            IsSuccess = false,
                            ErrorType = LoginErrorType.ServerError,
                            Message = "Server response format is invalid. Please try again or contact support.",
                            UserGuid = null,
                            UserStatus = null
                        };
                    }
                }
                catch (Exception parseEx)
                {
                    Console.WriteLine("Parse error: " + parseEx.Message);
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorType = LoginErrorType.ServerError,
                        Message = "Unable to process server response. Please try again.",
                        UserGuid = null,
                        UserStatus = null
                    };
                }
            }

            // Handle HTTP error status codes
            var errorMessage = response.StatusCode switch
            {
                System.Net.HttpStatusCode.Unauthorized => "Access denied. Please check your credentials.",
                System.Net.HttpStatusCode.NotFound => "Login service not found. Please try again later.",
                System.Net.HttpStatusCode.InternalServerError => "Server error occurred. Please try again later.",
                System.Net.HttpStatusCode.ServiceUnavailable => "Service temporarily unavailable. Please try again later.",
                System.Net.HttpStatusCode.BadRequest => "Invalid request. Please check your input.",
                _ => $"Login failed (HTTP {response.StatusCode}). Please try again later."
            };

            Console.WriteLine("Request failed with status code: " + response.StatusCode);
            return new LoginResult
            {
                IsSuccess = false,
                ErrorType = LoginErrorType.NetworkError,
                Message = errorMessage,
                UserGuid = null,
                UserStatus = null
            };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Network error: " + ex.Message);
            return new LoginResult
            {
                IsSuccess = false,
                ErrorType = LoginErrorType.NetworkError,
                Message = "Unable to connect to the server. Please check your internet connection and try again.",
                UserGuid = null,
                UserStatus = null
            };
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine("Timeout error: " + ex.Message);
            return new LoginResult
            {
                IsSuccess = false,
                ErrorType = LoginErrorType.Timeout,
                Message = "Request timed out. Please check your connection and try again.",
                UserGuid = null,
                UserStatus = null
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
            return new LoginResult
            {
                IsSuccess = false,
                ErrorType = LoginErrorType.Unknown,
                Message = "An unexpected error occurred. Please try again or contact support if the problem persists.",
                UserGuid = null,
                UserStatus = null
            };
        }
    }
}

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public LoginErrorType ErrorType { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? UserGuid { get; set; }
    public int? UserStatus { get; set; }
}

public enum LoginErrorType
{
    None,
    InvalidCredentials,
    AccountPending,
    NetworkError,
    Timeout,
    ServerError,
    Unknown
}