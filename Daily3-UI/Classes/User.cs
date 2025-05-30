namespace Daily3_UI.Classes;

/// <summary>
///     A simple class to show a user and the tickets that
///     are tied to the given user
/// </summary>
public class User
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="username">The name of the user</param>
    public User(string username, double totalWinnings)
    {
        Username = username;
        _totalWinnings = totalWinnings;
    }

    public User(string username)
    {
        Username = username;
    }

    public List<Ticket> Tickets = new();

    /// <summary>
    ///     The actual username the player goes under.
    ///     These should in theory be unique to every user.
    /// </summary>
    public string Username { get; init; }

    private readonly double _totalWinnings;

    /// <summary>
    ///     Gives you the total winnings of the user
    /// </summary>
    public string TotalWinnings => $"$ {_totalWinnings:F2}";
}