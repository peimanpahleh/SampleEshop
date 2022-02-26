namespace Products.Infrastructure.Persistence.Mongo;

public class MongoProductRepository : IProductRepository
{
    private readonly IMongoContext _context;
    private readonly IMongoCollection<Product> _dbSet;

    public MongoProductRepository(IMongoContext context, IOptions<ServiceSettings> settings)
    {
        _context = context;

        var mongo = settings.Value.ProductSettings.MongoStoreDatabase;

        if (string.IsNullOrEmpty(mongo.ProductCollectionName))
            throw new ArgumentNullException(nameof(mongo.ProductCollectionName));

        _dbSet = _context.GetCollection<Product>(mongo.ProductCollectionName);
    }

    public void Add(Product product) => _context.AddCommand(() => _dbSet.InsertOneAsync(product), product.DomainEvents);

    public void Update(Product product) =>
        _context.AddCommand(() => _dbSet.ReplaceOneAsync(item => item.Id == product.Id, product), product.DomainEvents);

    public Task<Product> GetByIdAsync(string productId) => _dbSet.Find(p => p.Id == productId).FirstOrDefaultAsync();


    public Task<bool> AnyAsync(string productId) => _dbSet.Find(p => p.Id == productId).AnyAsync();

    public void Dispose()
    {
        _context?.Dispose();
    }

    /* public async Task<string> AddAsync(Product product)
     {
         var find = await _productCollection.Find(p => p.Id == product.Id).FirstOrDefaultAsync();

         if (find != null)
             return find.Id;

         await _productCollection.InsertOneAsync(product);

         return product.Id;
     }

     public async Task<bool> ChangeNameAsync(string id, string name)
     {
         if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
             return false;

         var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();


         if (product == null)
             return false;

         product.ChangeName(name);


         var res = await _productCollection.ReplaceOneAsync(item => item.Id == id, product);

         return res.IsAcknowledged;
     }

     public async Task<bool> ChangePriceAsync(string id, long price)
     {
         if (string.IsNullOrEmpty(id) || price == 0)
             return false;

         var product = await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();


         if (product == null)
             return false;

         product.ChangePrice(price);

         var res = await _productCollection.ReplaceOneAsync(item => item.Id == id, product);

         return res.IsAcknowledged;
     }

     public async Task<Product> GetAsync(string productId)
     {
         var product = await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();
         return product;
     }

     public async Task<bool> UpdateAsync(Product product)
     {
         var find = await ProductExist(product.Id);

         if (!find)
             return false;

         var res = await _productCollection.ReplaceOneAsync(item => item.Id == product.Id, product);
         return res.IsAcknowledged;
     }

     private Task<bool> ProductExist(string productId) => _productCollection.Find(p => p.Id == productId).AnyAsync();*/

}
