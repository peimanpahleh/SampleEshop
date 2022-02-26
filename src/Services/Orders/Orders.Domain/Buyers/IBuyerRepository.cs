namespace Orders.Domain.Buyers;

public interface IBuyerRepository : IRepository<Buyer> , IDisposable
{
    void Add(Buyer buyer);
    Task<Buyer> GetByIdAsync(string buyerId);
    Task<Buyer> GetAsync(string buyerIdentityGuid);
    Task<bool> AnyAsync(string buyerId);
    void Update(Buyer buyer);

}
