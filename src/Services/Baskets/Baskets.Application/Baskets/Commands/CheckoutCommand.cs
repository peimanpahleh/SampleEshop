
using EventBus.IntegrationEvents.Baskets;

namespace Baskets.Application.Baskets.Commands;

public record CheckoutCommand(string CustomerId) : ICommand<bool>;

public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
{
    public CheckoutCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotNull();

        When(x => x.CustomerId != null, () =>
        {
            RuleFor(i => i.CustomerId.Length).GreaterThan(5);
        });
    }
}

public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IEventBus _bus;

    public CheckoutCommandHandler(IBasketRepository basketRepository, IEventBus bus)
    {
        _basketRepository = basketRepository;
        _bus = bus;
    }

    public async Task<bool> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.CustomerId))
            return false;

        var res = await _basketRepository.GetBasketAsync(Guid.Parse(request.CustomerId));

        if (res == null)
            return false;

        // publish event

        // UserCheckoutAcceptedIntegrationEvent
        var customerBasket = new CustomerBasket()
        {
            BuyerId = res.Id.ToString(),
            Items = res.BasketItems.Select(i => new CustomerBasketItem(
                i.Id.ToString(),
                i.ProductId,
                i.ProductName,
                i.UnitPrice,
                i.UnitPriceId,
                i.Quantity,
                i.ImageId)).ToList()
        };

        var msg = new UserCheckoutAccepted(res.Id.ToString(),res.BuyerName,customerBasket);

        await _bus.PublishAsync(msg);

        return true;

    }
}