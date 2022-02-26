using Orders.Application.Configuration.Events;
using Orders.Domain.Events;

namespace Orders.Application.DomainEventHandler;

public class UpdateOrderWhenBuyerVerifiedDomainEventHandler : IDomainEventHandler<BuyerVerifiedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderWhenBuyerVerifiedDomainEventHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BuyerVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(notification.OrderId);

        if (orderToUpdate == null)
            return;

        orderToUpdate.SetBuyerId(notification.BuyerId);

        // save to repo
        _orderRepository.Update(orderToUpdate);
        await _unitOfWork.SaveChangesAsync();

        // TODO make sure data is updated
    }
}


