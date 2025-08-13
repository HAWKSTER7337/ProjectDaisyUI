using System.Text.Json.Serialization;
using System.Web;
using Daily3_UI.Enums;

namespace Daily3_UI.Classes;

public class Ticket4 : Ticket
{
    [JsonPropertyName("Number4")] public int? Number4 { get; init; } = null;

    public override Color TicketColorTheme
    {
        get
        {
            Application.Current.Resources.TryGetValue("SecondaryDaily4", out var secondary);
            return (Color)secondary;
        }
    }

    public string FormattedNumber4 => Number4.ToString();

    public override string MissingMessage()
    {
        if (FormattedNumber4 is null) return "Missing a Fourth Number1";
        return base.MissingMessage();
    }

    protected override UriBuilder BuildApiUrl(string baseUrl, string endPoint)
    {
        var uriBuilder = base.BuildApiUrl(baseUrl, endPoint);
        var queryParmeters = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryParmeters["number4"] = FormattedNumber4;
        uriBuilder.Query = queryParmeters.ToString();
        return uriBuilder;
    }

    protected override string TextFormatFunction()
    {
        var information =
            $"{FormattedNumber1}-{FormattedNumber2}-{FormattedNumber3}-{FormattedNumber4}   Type: {Type}   {TimeOfDay}";
        if (WinningStatus == Enums.WinningStatus.Winner) information += $"\nPayout: {WinningPayout:F2}";

        return information;
    }

    public override Ticket4 ShallowCopy()
    {
        return (Ticket4)this.MemberwiseClone();
    }
    
    /// <summary>
    /// Creates a serializable Ticket4 object without PurchaseTimestamp
    /// </summary>
    public override SerializableTicket ToSerializableTicket()
    {
        return new SerializableTicket4
        {
            Number1 = this.Number1,
            Number2 = this.Number2,
            Number3 = this.Number3,
            Number4 = this.Number4,
            Price = this.Price ?? 0.0,
            Type = this.Type ?? TicketType.Straight,
            TimeOfDay = this.TimeOfDay ?? TOD.Midday,
            Date = this.Date ?? string.Empty,
            UserId = this.UserId ?? Guid.Empty
        };
    }
}