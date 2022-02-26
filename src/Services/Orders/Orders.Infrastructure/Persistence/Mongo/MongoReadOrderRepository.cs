namespace Orders.Infrastructure.Persistence.Mongo;

public class MongoReadOrderRepository : IReadOrderRepository
{

    private readonly IMongoCollection<Order> _orderCollection;

    public MongoReadOrderRepository(IMongoDatabase db, IOptions<ServiceSettings> serviceSettings)
    {
        var mongo = serviceSettings.Value?.OrderSettings.MongoStoreDatabase;

        _orderCollection = db.GetCollection<Order>(
           mongo.OrderCollectionName);
    }

    public async Task<PagedList<Order>> GetAllOrder(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var results = await _orderCollection.AggregateByPage(
            Builders<Order>.Filter.Empty,
            Builders<Order>.Sort.Descending(x => x.OrderDate),
            page: pageIndex,
            pageSize: pageSize);


        return new PagedList<Order>()
        {
            TotalItems = results.totalItems,
            TotalPages = results.totalPages,
            Result = results.data
        };
    }

    public async Task<PagedList<Order>> GetAllOrderByBuyerId(string buyerId, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var results = await _orderCollection.AggregateByPage(
            Builders<Order>.Filter.Where(x => x.BuyerId == buyerId),
            Builders<Order>.Sort.Descending(x => x.OrderDate),
            page: pageIndex,
            pageSize: pageSize);


        return new PagedList<Order>()
        {
            TotalItems = results.totalItems,
            TotalPages = results.totalPages,
            Result = results.data
        };
    }

    public async Task<Order> GetOrderById(string orderId, CancellationToken cancellationToken = default)
    {
        var result = await _orderCollection.Find(p => p.Id == orderId)
           .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
