using Products.Infrastructure.Settings;

namespace Products.Infrastructure.Persistence.Mongo;

public class MongoReadProductRepository : IReadProductRepository
{
    private readonly IMongoCollection<Product> _productCollection;

    public MongoReadProductRepository(IMongoDatabase db, IOptions<ServiceSettings> settings)
    {
        var mongo = settings.Value.ProductSettings.MongoStoreDatabase;

        _productCollection = db.GetCollection<Product>(
            mongo.ProductCollectionName);
    }

    public async Task<PagedList<Product>> GetAllProduct(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var results = await _productCollection.AggregateByPage(
        Builders<Product>.Filter.Empty,
        Builders<Product>.Sort.Ascending(x => x.Id),
        page: pageIndex,
        pageSize: pageSize);

        return new PagedList<Product>()
        {
            TotalItems = results.totalItems,
            TotalPages = results.totalPages,
            Result = results.data
        };
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<string> Ids, CancellationToken cancellationToken = default)
    {
        var filterDef = new FilterDefinitionBuilder<Product>();
        var filter = filterDef.In(x => x.Id, Ids);
        var res = await _productCollection.Find(filter).ToListAsync();
        return res;
    }

    public async Task<Product> GetProductById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _productCollection.Find(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

}
