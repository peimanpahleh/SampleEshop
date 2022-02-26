namespace Products.Infrastructure.Persistence.Mongo;

public class MongoImageRepository : IImageRepository
{
    private readonly IMongoContext _context;
    private readonly IMongoCollection<Image> _dbSet;

    public MongoImageRepository(IMongoContext context, IOptions<ServiceSettings> settings)
    {
        _context = context;

        var mongo = settings.Value.ProductSettings.MongoStoreDatabase;

        if (string.IsNullOrEmpty(mongo.ImageCollectionName))
            throw new ArgumentNullException(nameof(mongo.ImageCollectionName));

        _dbSet = _context.GetCollection<Image>(mongo.ImageCollectionName);
    }

    public void Add(Image image) => _context.AddCommand(() => _dbSet.InsertOneAsync(image), image.DomainEvents);

    public Task<Image> GetByIdAsync(string imageId) => _dbSet.Find(i => i.Id == imageId).FirstOrDefaultAsync();


    public Task<bool> AnyAsync(string imageId) => _dbSet.Find(i => i.Id == imageId).AnyAsync();

    public void Dispose()
    {
        _context?.Dispose();
    }

    /*public async Task<string> AddAsync(Image image)
    {
        await _imageCollection.InsertOneAsync(image);

        return image.Id;
    }

    public Task<bool> AnyAsync(string Id) => _imageCollection.Find(x => x.Id == Id).AnyAsync();


    public async Task<Image> GetAsync(string Id)
    {
        var find = await _imageCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

        return find;
    }

    public async Task<List<Image>> GetAsyncByIds(IEnumerable<string> Ids)
    {
        var filterDef = new FilterDefinitionBuilder<Image>();
        var filter = filterDef.In(x => x.Id, Ids);
        var res = await _imageCollection.Find(filter).ToListAsync();
        return res;
    }*/
}
