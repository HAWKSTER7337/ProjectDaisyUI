using System.Collections;

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
    public User(string username)
    {
        Username = username;
        Tickets = new AddOnlyList<Ticket>();
    }

    /// <summary>
    ///     The actual username the player goes under.
    ///     These should in theory be unique to every user.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    ///     The tickets that are connected to the given user
    /// </summary>
    public AddOnlyList<Ticket> Tickets { get; }

    /// <summary>
    ///     Gives you the total winnings of the user
    /// </summary>
    public string TotalWinnings => $"$ {CalculateTotalWinnings()}";

    /// <summary>
    ///     This is a function that will run though all the tickets that the player
    ///     has in play for the given period and return the sum of the total earnings
    /// </summary>
    /// <returns>Total earnings</returns>
    private float CalculateTotalWinnings()
    {
        return 100;
    }
}

public class AddOnlyList<T> : IEnumerable<T>
{
    private readonly List<T> _list = new();

    public void Add(T item)
    {
        _list.Add(item);
    }

    public List<T> ToList()
    {
        return _list.ToList();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}