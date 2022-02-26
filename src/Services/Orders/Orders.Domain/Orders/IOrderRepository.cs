namespace Orders.Domain.Orders;

public interface IOrderRepository : IRepository<Order> , IDisposable
{
    void Add(Order order);
    Task<Order> GetByIdAsync(string orderId);
    Task<bool> AnyAsync(string orderId);
    void Update(Order order);
}
