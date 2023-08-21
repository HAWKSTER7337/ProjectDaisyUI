using System.Web;
using Daily3_UI.Enums;

namespace Daily3_UI.Classes;

/// <summary>
///     The storage system of Tickets
/// </summary>
public class Ticket
{
    // Winning Numbers
    public string Number1 { get; init; } = null;
    public string Number2 { get; init; } = null;
    public string Number3 { get; init; } = null;

    // Specifications 
    public string Price { get; init; } = null;
    public TicketType? Type { get; init; } = null;
    public TOD? TimeOfDay { get; init; } = null;
    public string Date { get; init; } = null;

    /// <summary>
    ///     returns a message if something was set as null
    /// </summary>
    public string MissingMessage()
    {
        if (Number1 is null) return "Missing a First Number!";
        if (Number2 is null) return "Missing a Second Number!";
        if (Number3 is null) return "Missing a Third Number!";
        if (Price is null) return "Pick a Price!";
        if (Type is null) return "Pick a Ticket Type!";
        if (TimeOfDay is null) return "Time of day has not been entered!";
        return Date is null ? "No Date Entered" : null;
    }

    /// <summary>
    ///     Turning values into something readable by the API
    /// </summary>
    public string ToApiUrl(string baseUrl, string endPoint)
    {
        // Building Params
        var uriBuilder = new UriBuilder(baseUrl + endPoint);
        var queryParameters = HttpUtility.ParseQueryString(string.Empty);
        queryParameters["number1"] = Number1;
        queryParameters["number2"] = Number2;
        queryParameters["number3"] = Number3;
        queryParameters["price"] = Price;
        queryParameters["ticketType"] = ((int)Type).ToString();
        queryParameters["timeOfDay"] = ((int)TimeOfDay).ToString();
        queryParameters["date"] = Date;
        queryParameters["userId"] = Globals.UserId.ToString();
        uriBuilder.Query = queryParameters.ToString();
        return uriBuilder.ToString();
    }

    /// <summary>
    ///     return the number for it to appear on screen
    /// </summary>
    public string TextFormat => $"{Number1}-{Number2}-{Number3}   Type: {Type}   TOD: {TimeOfDay}";

    /// <summary>
    ///     returns the details format of the tickets bought
    /// </summary>
    public string DetailsFormat => $"Price: ${Price}   Date: {DateTime.Parse(Date).ToString("MM/dd/yyyy")}";
}