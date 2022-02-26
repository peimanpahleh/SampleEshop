namespace Baskets.Infrastructure.Settings;

public class BasketSettings
{
    public string RedisConnection { get; set; }
    public ConsulSettings ConsulSettings { get; set; }

}