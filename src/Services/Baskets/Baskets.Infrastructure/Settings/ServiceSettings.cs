namespace Baskets.Infrastructure.Settings;

public class ServiceSettings
{
    public string IdentityUrl { get; set; }
    public string Consul { get; set; }
    public GprcUrls GprcUrls { get; set; }
    public MassTransitSettings MassTransitSettings { get; set; }

    public BasketSettings BasketSettings { get; set; }

}
