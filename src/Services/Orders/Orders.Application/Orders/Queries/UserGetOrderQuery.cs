
namespace Orders.Application.Orders.Queries;

public record UserGetOrderQuery(string OrderId) : IQuery<UserOrderDto>;

public class UserGetOrderQueryHandler : IQueryHandler<UserGetOrderQuery, UserOrderDto>
{
    private readonly IReadOrderRepository _orderRepository;
    private readonly IIdentityService _identityService;

    public UserGetOrderQueryHandler(IReadOrderRepository orderRepository, IIdentityService identityService)
    {
        _orderRepository = orderRepository;
        _identityService = identityService;
    }
    public async Task<UserOrderDto> Handle(UserGetOrderQuery request, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdentity();

        var res = await _orderRepository
           .GetOrderById(request.OrderId, cancellationToken);

        if (res.BuyerId != userId)
            return null;

        if (res == null)
            return null;

        var items = res.OrderItems
            .Select(x => new UserOrderItemDto(x.Id,x.ProductId, x.ProductName, x.UnitPrice,x.ImageId));

        var mapped = new UserOrderDto(res.Id, res.OrderStatus.Name, items);

        return mapped;
    }
}
