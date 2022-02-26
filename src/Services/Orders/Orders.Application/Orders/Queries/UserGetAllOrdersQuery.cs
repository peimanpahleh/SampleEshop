
namespace Orders.Application.Orders.Queries;

public record UserGetAllOrdersQuery(int PageIndex, int PageSize) : IQuery<PagedList<UserAllOrdersDto>>;


public class UserGetAllOrdersQueryHandler : IQueryHandler<UserGetAllOrdersQuery, PagedList<UserAllOrdersDto>>
{
    private readonly IReadOrderRepository _orderRepository;
    private readonly IIdentityService _identityService;

    public UserGetAllOrdersQueryHandler(IReadOrderRepository orderRepository, IIdentityService identityService)
    {
        _orderRepository = orderRepository;
        _identityService = identityService;
    }

    public async Task<PagedList<UserAllOrdersDto>> Handle(UserGetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        if (!_identityService.IsAuthenticated())
            return new PagedList<UserAllOrdersDto>
            {
                TotalItems = 0,
                TotalPages = 0,
                Result = new List<UserAllOrdersDto>()
            };

        var userId = _identityService.GetUserIdentity();

        var res = await _orderRepository
            .GetAllOrderByBuyerId(userId, request.PageIndex, request.PageSize, cancellationToken);

        List<UserAllOrdersDto> items = new();

        // map from Order to UserAllOrdersDto
        foreach (var order in res.Result)
        {
            var item = new UserAllOrdersDto(order.Id, order.GetTotalPrice(), order.OrderItems.Count, order.OrderStatus.Name);
            items.Add(item);
        }
        // var items =  res.Result.Select(o => new UserAllOrdersDto(o.Id,0,res.Result.Count));

        return new PagedList<UserAllOrdersDto>
        {
            TotalItems = res.TotalItems,
            TotalPages = res.TotalPages,
            Result = items
        };
    }
}

