namespace Orders.Application.IntegrationEvents.EventHandler;

public class OrderStatusChangedToStockConfirmedHandler : IIntegrationEventHandler<OrderStatusChangedToStockConfirmed>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStatusChangedToStockConfirmedHandler> _logger;

    public OrderStatusChangedToStockConfirmedHandler(IMediator mediator, ILogger<OrderStatusChangedToStockConfirmedHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToStockConfirmed> context)
    {
        var msg = context.Message;

        _logger.LogInformation($"OrderStatusChangedToStockConfirmedHandler received msg for orderId:{msg.OrderId}");


        var command = new SetStockConfirmedOrderStatusCommand(msg.OrderId);
        await _mediator.Send(command);
    }
}
