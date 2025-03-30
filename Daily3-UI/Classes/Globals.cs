using Daily3_UI.Enums;

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
}