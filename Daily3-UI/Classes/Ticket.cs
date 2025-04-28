using System.Text.Json.Serialization;
using System.Web;
using Daily3_UI.Enums;

namespace Daily3_UI.Classes;

/// <summary>
///     The storage system of Tickets
/// </summary>
public abstract class Ticket
{
    // Playing Numbers
    [JsonPropertyName("Number1")] public int? Number1 { get; init; } = null;

    [JsonPropertyName("Number2")] public int? Number2 { get; init; } = null;

    [JsonPropertyName("Number3")] public int? Number3 { get; init; } = null;

    // Specifications 
    [JsonPropertyName("Price")] public double? Price { get; init; } = null;

    [JsonPropertyName("Type")] public TicketType? Type { get; init; } = null;

    [JsonPropertyName("TimeOfDay")] public TOD? TimeOfDay { get; init; } = null;

    [JsonPropertyName("Date")] public string Date { get; init; } = null;


    [JsonPropertyName("WinningStatus")] public WinningStatus? WinningStatus { get; init; } = Enums.WinningStatus.TBD;

    [JsonPropertyName("WinningPayout")] public double WinningPayout { get; init; }
    [JsonPropertyName("UserId")] public Guid? UserId { get; set; } = null;

    [JsonIgnore] public abstract Color TicketColorTheme { get; }

    // Format Functions
    [JsonIgnore] public string FormattedNumber1 => Number1.ToString();
    [JsonIgnore] public string FormattedNumber2 => Number2.ToString();
    [JsonIgnore] public string FormattedNumber3 => Number3.ToString();
    [JsonIgnore] public string FormattedPrice => Price.HasValue ? Price.Value.ToString("0.00") : "N/A";

    /// <summary>
    ///     returns a message if something was set as null
    ///     This is for buying tickets so only values that
    ///     are picked by the user should be here
    /// </summary>
    public virtual string MissingMessage()
    {
        if (Number1 is null) return "Missing a First Number!";
        if (Number2 is null) return "Missing a Second Number!";
        if (Number3 is null) return "Missing a Third Number!";
        if (FormattedPrice is null) return "Pick a Price!";
        if (Type is null) return "Pick a Ticket Type!";
        if (TimeOfDay is null) return "Time of day has not been entered!";
        return Date is null ? "No Date Entered" : null;
    }

    /// <summary>
    ///     Turning values into something readable by the API
    /// </summary>
    public string ToApiUrl(string baseUrl, string endPoint)
    {
        var uriBuilder = BuildApiUrl(baseUrl, endPoint);
        return uriBuilder.ToString();
    }

    protected virtual UriBuilder BuildApiUrl(string baseUrl, string endPoint)
    {
        // Building Params
        var uriBuilder = new UriBuilder(baseUrl + endPoint);
        var queryParameters = HttpUtility.ParseQueryString(string.Empty);
        queryParameters["number1"] = FormattedNumber1;
        queryParameters["number2"] = FormattedNumber2;
        queryParameters["number3"] = FormattedNumber3;
        queryParameters["price"] = FormattedPrice;
        queryParameters["ticketType"] = ((int)Type).ToString();
        queryParameters["timeOfDay"] = ((int)TimeOfDay).ToString();
        queryParameters["date"] = Date;
        queryParameters["userId"] = Globals.UserId.ToString();
        uriBuilder.Query = queryParameters.ToString();
        return uriBuilder;
    }

    /// <summary>
    ///     return the number for it to appear on screen
    /// </summary>
    protected virtual string TextFormatFunction()
    {
        var information = $"{FormattedNumber1}-{FormattedNumber2}-{FormattedNumber3}   Type: {Type}   TOD: {TimeOfDay}";
        if (WinningStatus == Enums.WinningStatus.Winner) information += $"\nPayout: {WinningPayout:F2}";

        return information;
    }

    public string TextFormat => TextFormatFunction();

    /// <summary>
    ///     returns the details format of the tickets bought
    /// </summary>
    public string DetailsFormat =>
        $"Price: ${FormattedPrice}   Date: {DateTime.Parse(Date):MM/dd/yyyy}";
}