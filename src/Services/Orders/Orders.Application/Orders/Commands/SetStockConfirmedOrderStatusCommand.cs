namespace Orders.Application.Orders.Commands;

public record SetStockConfirmedOrderStatusCommand(string OrderId) : ICommand<bool>;

public class SetStockConfirmedOrderStatusCommandHandler :
    ICommandHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
            return false;

        order.SetStockConfirmedStatus();

        _orderRepository.Update(order);
        var res =  await _unitOfWork.SaveChangesAsync(cancellationToken);

        return res;

    }
}