namespace Daily3_UI.Clients.ClientRequests;

public class ChangePasswordRequest
{
    public Credentials Credentials { get; set; }
    public string NewPassword { get; set; }
}