
namespace Orders.Application.IntegrationEvents.EventHandler;

public class UserCheckoutAcceptedHandler : IIntegrationEventHandler<UserCheckoutAccepted>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserCheckoutAcceptedHandler> _logger;

    public UserCheckoutAcceptedHandler(IMediator mediator, ILogger<UserCheckoutAcceptedHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCheckoutAccepted> context)
    {
        var msg = context.Message;

        // map CustomerBasket to OrderItems
        var orderItems = msg.Basket.Items.Select(x => new OrderItemDto(
            x.Id,
            x.ProductId,
            x.ProductName,
            x.UnitPrice,
            x.UnitPriceId,
            x.Quantity,
            x.ImageId)).ToList();

        var createOrderCommand = new CreateOrderCommand(msg.UserId, msg.UserName, orderItems);

        var res = await _mediator.Send(createOrderCommand);

        if (res)
        {
            _logger.LogInformation("----- CreateOrderCommand suceeded - RequestId: {RequestId}", msg.Id);
        }
        else
        {
            _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", msg.Id);
        }

    }
}
