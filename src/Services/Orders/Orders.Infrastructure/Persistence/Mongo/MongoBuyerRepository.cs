using Orders.Infrastructure.Settings;

namespace Orders.Infrastructure.Persistence.Mongo;

public class MongoBuyerRepository : IBuyerRepository
{

    private readonly IMongoContext Context;
    private readonly IMongoCollection<Buyer> DbSet;

    public MongoBuyerRepository(IMongoContext context, IOptions<ServiceSettings> serviceSettings)
    {
        Context = context;

        var mongo = serviceSettings.Value?.OrderSettings.MongoStoreDatabase;

        if (string.IsNullOrEmpty(mongo.BuyerCollectionName))
            throw new ArgumentNullException(nameof(mongo.BuyerCollectionName));

        DbSet = Context.GetCollection<Buyer>(mongo.BuyerCollectionName);
    }

    public void Add(Buyer buyer) => Context.AddCommand(() => DbSet.InsertOneAsync(buyer),buyer.DomainEvents);


    public Task<Buyer> GetByIdAsync(string buyerId) => DbSet.Find(p => p.Id == buyerId).FirstOrDefaultAsync();
    public Task<Buyer> GetAsync(string buyerIdentityGuid) 
        => DbSet.Find(p => p.IdentityGuid == buyerIdentityGuid).FirstOrDefaultAsync();

    public Task<bool> AnyAsync(string buyerId) => DbSet.Find(o => o.Id == buyerId).AnyAsync();

    public void Update(Buyer buyer)
    {
        Context.AddCommand(() => DbSet.ReplaceOneAsync(item => item.Id == buyer.Id, buyer),buyer.DomainEvents);
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}
