using System.Text.Json.Serialization;
using System.Web;

namespace Daily3_UI.Classes;

public class Ticket4 : Ticket
{
    // Extra Playing number
    [JsonPropertyName("Number4")] public int? Number4 { get; init; } = null;

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

    public override string TextFormat =>
        $"{FormattedNumber1}-{FormattedNumber2}-{FormattedNumber3}-{FormattedNumber4}   Type: {Type}   TOD: {TimeOfDay}";
}