using EventBus.IntegrationEvents.Payments;
using Microsoft.Extensions.Logging;

namespace Payments.Application.IntegrationEventsHandler;

public class OrderStatusChangedToStockConfirmedHandler : IIntegrationEventHandler<OrderStatusChangedToStockConfirmed>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<OrderStatusChangedToStockConfirmedHandler> _logger;
    public OrderStatusChangedToStockConfirmedHandler(IEventBus eventBus,
        ILogger<OrderStatusChangedToStockConfirmedHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToStockConfirmed> context)
    {

        // for test
        bool paymentSucceeded = true;

        var msg = context.Message;

        _logger.LogInformation($"msg received orderId:{msg.OrderId}");

        // for simulate banking ...
        //await Task.Delay(10000);
        await Task.Delay(3000);

        if (paymentSucceeded)
        {
            var orderPaymentSucceeded = new OrderPaymentSucceeded(msg.OrderId);
            await _eventBus.PublishAsync(orderPaymentSucceeded);

            _logger.LogInformation($"OrderPaymentSucceeded published orderId:{msg.OrderId}");

        }
        else
        {
            var orderPaymentFailed = new OrderPaymentFailed(msg.OrderId);
            await _eventBus.PublishAsync(orderPaymentFailed);
        }

    }
}
