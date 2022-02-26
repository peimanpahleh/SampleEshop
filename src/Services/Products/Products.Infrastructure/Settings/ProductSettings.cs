namespace Products.Infrastructure.Settings;

public class ProductSettings
{
    public MongoStoreDatabase MongoStoreDatabase { get; set; }
    public ConsulSettings ConsulSettings { get; set; }
}
