namespace Orders.Infrastructure.Settings;

public class MongoStoreDatabase
{
    public string DatabaseName { get; set; } = null!;

    public string BuyerCollectionName { get; set; } = null!;
    public string OrderCollectionName { get; set; } = null!;
}
