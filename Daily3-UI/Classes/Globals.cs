namespace Daily3_UI.Classes;

/// <summary>
///     All Global values to the application
/// </summary>
public static class Globals
{
    /// <summary>
    ///     The userID Value
    /// </summary>
    private static int? _userid;

    /// <summary>
    ///     functions allowing you to only change the value of your UserID
    ///     once. This is to make sure you are not changing the value in some
    ///     weird way for security
    /// </summary>
    public static int? UserId
    {
        get => _userid;
        set => _userid ??= value;
    }
}