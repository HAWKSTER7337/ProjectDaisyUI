using Daily3_UI.Enums;
using Daily3_UI.Pages;

namespace Daily3_UI.Classes;

/// <summary>
///     All Global values to the application
/// </summary>
public static class Globals
{
    /// <summary>
    ///     The userID Value
    /// </summary>
    private static Guid? _userid;

    /// <summary>
    ///     The status ranking of the user
    /// </summary>
    private static Status? _status;

    /// <summary>
    ///     functions allowing you to only change the value of your UserID
    ///     once. This is to make sure you are not changing the value in some
    ///     weird way for security
    /// </summary>
    public static Guid? UserId
    {
        get => _userid;
        set => _userid ??= value;
    }

    public static Status? Status
    {
        get => _status;
        set => _status ??= value;
    }

    /// <summary>
    ///     Trys to get color from static resources
    ///     returns white if the color is not found
    /// </summary>
    public static Color GetColor(string colorName)
    {
        var resourceColor = Application.Current.Resources.TryGetValue(colorName, out var value) && value is Color color
            ? color
            : Color.FromRgb(0, 0, 0);
        return resourceColor;
    }

    public static List<KeyValuePair<string, string>> Daily3ContentPages = new()
    {
        new ("Daily Numbers", "Home3"),
        new ("Buy Tickets", "Buy3"),
        new ("View History", "History3"),
        new ("Entrants Tickets", "Entrants3")
    };

    public static List<KeyValuePair<string, string>> Daily4ContentPages = new()
    {
        new ("Daily Numbers", "Home4"),
        new ("Buy Tickets", "Buy4"),
        new ("View History", "History4"),
        new ("Entrants Tickets", "Entrants4")
    };
}