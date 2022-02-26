namespace Orders.Application.IntegrationEvents.EventHandler;

public class OrderStatusChangedToCancelledHandler : IIntegrationEventHandler<OrderStatusChangedToCancelled>
{
    private readonly IMediator _mediator;

    public OrderStatusChangedToCancelledHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToCancelled> context)
    {
        var msg = context.Message;
        var command = new CancelOrderCommand(msg.OrderId);
        await _mediator.Send(command);
    }
}
