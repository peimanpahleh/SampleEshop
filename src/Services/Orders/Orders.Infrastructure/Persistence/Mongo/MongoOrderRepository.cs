using Orders.Infrastructure.Settings;

namespace Orders.Infrastructure.Persistence.Mongo;

public class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoContext Context;
    private readonly IMongoCollection<Order> DbSet;

    public MongoOrderRepository(IMongoContext context, IOptions<ServiceSettings> serviceSettings)
    {
        Context = context;

        var mongo = serviceSettings.Value?.OrderSettings.MongoStoreDatabase;

        if (string.IsNullOrEmpty(mongo.OrderCollectionName))
            throw new ArgumentNullException(nameof(mongo.OrderCollectionName));

        DbSet = Context.GetCollection<Order>(mongo.OrderCollectionName);
    }

    public void Add(Order order) => Context.AddCommand(() => DbSet.InsertOneAsync(order),order.DomainEvents);

    public Task<Order> GetByIdAsync(string orderId) => DbSet.Find(p => p.Id == orderId).FirstOrDefaultAsync();

    public Task<bool> AnyAsync(string orderId) => DbSet.Find(o => o.Id == orderId).AnyAsync();

    public void Update(Order order)
    {
        Context.AddCommand(() => DbSet.ReplaceOneAsync(item => item.Id == order.Id, order),order.DomainEvents);
    }

    public void Dispose()
    {
        Context?.Dispose();
    }


}
