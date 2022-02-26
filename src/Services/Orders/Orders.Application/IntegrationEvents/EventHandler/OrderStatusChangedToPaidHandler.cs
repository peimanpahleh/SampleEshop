namespace Orders.Application.IntegrationEvents.EventHandler;

public class OrderStatusChangedToPaidHandler : IIntegrationEventHandler<OrderStatusChangedToPaid>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStatusChangedToPaidHandler> _logger;

    public OrderStatusChangedToPaidHandler(IMediator mediator, ILogger<OrderStatusChangedToPaidHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToPaid> context)
    {
        var msg = context.Message;

        _logger.LogInformation($"OrderStatusChangedToPaidHandler received msg orderId:{msg.OrderId}");

        var command = new SetPaidOrderStatusCommand(msg.OrderId);
        await _mediator.Send(command);
    }
}
