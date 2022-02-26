using Orders.Application.Configuration.Events;
using Orders.Domain.Buyers;
using Orders.Domain.Events;

namespace Orders.Application.DomainEventHandler;

public class ValidateOrAddBuyerWhenOrderStartedDomainEventHandler : IDomainEventHandler<OrderStartedDomainEvent>
{
    private readonly IBuyerRepository _buyerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ValidateOrAddBuyerWhenOrderStartedDomainEventHandler(IBuyerRepository buyerRepository, IUnitOfWork unitOfWork)
    {
        _buyerRepository = buyerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var buyer = await _buyerRepository.GetAsync(notification.UserId);
        bool buyerOriginallyExisted = (buyer == null) ? false : true;

        if (!buyerOriginallyExisted)
        {
            buyer = new Buyer(notification.UserId, notification.UserName);
        }

        buyer.VerifyBuyer(buyer.Id, notification.OrderId);


        // buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.Add(buyer);

        if (buyerOriginallyExisted)
            _buyerRepository.Update(buyer);
        else
            _buyerRepository.Add(buyer);

        await _unitOfWork.SaveChangesAsync();


    }
}


