namespace Orders.Infrastructure.Settings;

public class OrderSettings
{
    public MongoStoreDatabase MongoStoreDatabase { get; set; }
    public ConsulSettings ConsulSettings { get; set; }

}
