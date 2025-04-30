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
}