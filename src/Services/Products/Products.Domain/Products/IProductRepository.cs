namespace Products.Domain.Products;

public interface IProductRepository : IRepository<Product>, IDisposable
{
    void Add(Product product);
    Task<Product> GetByIdAsync(string productId);
    Task<bool> AnyAsync(string productId);
    void Update(Product product);
}
