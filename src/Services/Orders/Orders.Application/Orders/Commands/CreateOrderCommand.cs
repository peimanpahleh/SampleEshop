namespace Orders.Application.Orders.Commands;

public record CreateOrderCommand(string UserId,string UserName, List<OrderItemDto> OrderItems) : ICommand<bool>;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // map OrderItemDto to OrderItem
        var orderItems = request.OrderItems.Select(x => new OrderItem(
            x.Id,
            x.ProductId,
            x.ProductName,
            x.UnitPrice,
            x.UnitPriceId,
            x.ImageId,
            x.Quantity)).ToList();


        var order = new Order(request.UserId,request.UserName, orderItems);

        var findOrder = await _orderRepository.AnyAsync(order.Id);
        if (findOrder)
        {
            // order already existed
            return false;
        }

        _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // make sure data is added
        findOrder = await _orderRepository.AnyAsync(order.Id);
        if (!findOrder)
        {
            // can not add order
            return false;
        }

        // map OrderItemDto to OrderStockItem
        var customerBasketItem = request.OrderItems.Select(x => new CustomerBasketItem(
            x.Id,
            x.ProductId,
            x.ProductName,
            x.UnitPrice,
            x.UnitPriceId,
            x.Quantity,
            x.ImageId)).ToList();

        var orderStarted = new OrderStarted(order.Id,request.UserId,request.UserName,customerBasketItem);
        await _eventBus.PublishAsync(orderStarted);

        return true;
    }
}
