namespace Orders.Application.Orders.Commands;

public record SetAwaitingValidationOrderStatusCommand(string OrderId) : ICommand<bool>;

public class SetAwaitingValidationOrderStatusCommandHandler : 
    ICommandHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetAwaitingValidationOrderStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetAwaitingValidationOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
            return false;

        order.SetAwaitingValidationStatus();

        _orderRepository.Update(order);
       var res = await _unitOfWork.SaveChangesAsync();

        return res;

    }
}