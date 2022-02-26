namespace Orders.Application.Orders.Commands;

public record CancelOrderCommand(string OrderId) : ICommand<bool>;


public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.OrderId);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetCancelledStatus();

        _orderRepository.Update(orderToUpdate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO make sure data is updated

        return true;
    }
}
