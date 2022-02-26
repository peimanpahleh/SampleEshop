namespace Payments.Infrastructure;

public class ServiceSettings
{
    public string IdentityUrl { get; set; }
    public string Consul { get; set; }
    public MassTransitSettings MassTransitSettings { get; set; }

}
