namespace Orders.Infrastructure.Settings;

public class ServiceSettings
{
    public string IdentityUrl { get; set; }
    public string Consul { get; set; }
    public string MongoDbConnection { get; set; }
    public MassTransitSettings MassTransitSettings { get; set; }
    public OrderSettings OrderSettings { get; set; }

}
