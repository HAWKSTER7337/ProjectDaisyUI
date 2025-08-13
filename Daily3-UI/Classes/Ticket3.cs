using Daily3_UI.Enums;

namespace Daily3_UI.Classes;

public class Ticket3 : Ticket
{
    public override Color TicketColorTheme
    {
        get
        {
            Application.Current.Resources.TryGetValue("Secondary", out var secondary);
            return (Color)secondary;
        }
    }
    
    public override Ticket3 ShallowCopy()
    {
        return (Ticket3)this.MemberwiseClone();
    }
    
    /// <summary>
    /// Creates a serializable Ticket3 object without PurchaseTimestamp
    /// </summary>
    public override SerializableTicket ToSerializableTicket()
    {
        return new SerializableTicket
        {
            Number1 = this.Number1,
            Number2 = this.Number2,
            Number3 = this.Number3,
            Price = this.Price ?? 0.0,
            Type = this.Type ?? TicketType.Straight,
            TimeOfDay = this.TimeOfDay ?? TOD.Midday,
            Date = this.Date ?? string.Empty,
            UserId = this.UserId ?? Guid.Empty
        };
    }
}