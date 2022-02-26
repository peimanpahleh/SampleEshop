namespace Products.Infrastructure.Settings;

public class MongoStoreDatabase
{
    public string DatabaseName { get; set; } = null!;
    public string ProductCollectionName { get; set; } = null!;
    public string ImageCollectionName { get; set; } = null!;
}
