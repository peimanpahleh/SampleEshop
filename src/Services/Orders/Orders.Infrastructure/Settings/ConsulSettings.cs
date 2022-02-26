namespace Orders.Infrastructure.Settings;

public class ConsulSettings
{
    public string Address { get; set; }
    public string Name { get; set; }
    public int Port { get; set; }
    public string Ping { get; set; }
    public int GrpcPort { get; set; }

}
