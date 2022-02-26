
namespace Orders.Application.ReadSide;

public interface IReadOrderRepository
{
    Task<PagedList<Order>> GetAllOrder(int pageIndex, int pageSize, CancellationToken cancellationToken = default);

    Task<PagedList<Order>> GetAllOrderByBuyerId(string buyerId,int pageIndex, int pageSize, CancellationToken cancellationToken = default);

    Task<Order> GetOrderById(string orderId,CancellationToken cancellationToken = default);

}
