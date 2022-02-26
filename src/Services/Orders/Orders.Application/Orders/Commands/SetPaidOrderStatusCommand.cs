namespace Orders.Application.Orders.Commands;

public record SetPaidOrderStatusCommand(string OrderId) : ICommand<bool>;

public class SetPaidOrderStatusCommandHandler :
    ICommandHandler<SetPaidOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetPaidOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
            return false;

        order.SetPaidStatus();

        _orderRepository.Update(order);
        var res =  await _unitOfWork.SaveChangesAsync(cancellationToken);
        return res;

    }
}