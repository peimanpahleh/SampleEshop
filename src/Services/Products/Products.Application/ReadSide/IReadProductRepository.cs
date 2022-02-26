
namespace Products.Application.ReadSide;

public interface IReadProductRepository
{
    Task<PagedList<Product>> GetAllProduct(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    Task<Product> GetProductById(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<string> Ids, CancellationToken cancellationToken = default);
}
