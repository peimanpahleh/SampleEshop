using Baskets.Application.Baskets.Commands;

namespace Baskets.Application.IntegrationEvents.EventHandler;

public class OrderStartedHandler : IIntegrationEventHandler<OrderStarted>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStartedHandler> _logger;

    public OrderStartedHandler(
        IMediator mediator,
    ILogger<OrderStartedHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OrderStarted> context)
    {
        var msg = context.Message;

        _logger.LogInformation($"OrderStartedHandler received msg for orderId:{msg.OrderId}");

        var command = new DeleteBasketCommand(msg.BuyerId);
        await _mediator.Send(command);

    }
}
