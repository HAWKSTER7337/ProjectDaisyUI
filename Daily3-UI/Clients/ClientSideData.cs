namespace Daily3_UI.Clients;

public static class ClientSideData
{
    private static readonly string PortNumber = "5198";

    //private static readonly string IpAddress = "10.0.2.2"; // Local Testing
    //private static string IpAddress = "10.0.0.105"; // Home Testing
    private static readonly string IpAddress = "192.168.137.2"; // linux server

    public static string BaseUrl => $"http://{IpAddress}:{PortNumber}/";
}