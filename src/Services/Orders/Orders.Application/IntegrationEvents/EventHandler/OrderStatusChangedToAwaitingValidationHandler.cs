namespace Orders.Application.IntegrationEvents.EventHandler;

public class OrderStatusChangedToAwaitingValidationHandler : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidation>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationHandler> _logger;

    public OrderStatusChangedToAwaitingValidationHandler(IMediator mediator, ILogger<OrderStatusChangedToAwaitingValidationHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToAwaitingValidation> context)
    {

        var msg = context.Message;

        _logger.LogInformation($"OrderStatusChangedToAwaitingValidationHandler received msg for orderId:{msg.OrderId}");

        var command = new SetAwaitingValidationOrderStatusCommand(msg.OrderId);
        await _mediator.Send(command);
    }
}
