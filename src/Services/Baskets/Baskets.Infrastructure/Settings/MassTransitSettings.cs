namespace Baskets.Infrastructure.Settings;

public class MassTransitSettings
{
    public string RabbitmqHost { get; set; } = null!;
    public string RabbitmqVirtualHost { get; set; } = null!;
    public string RabbitmqUsername { get; set; } = null!;
    public string RabbitmqPassword { get; set; } = null!;
}
