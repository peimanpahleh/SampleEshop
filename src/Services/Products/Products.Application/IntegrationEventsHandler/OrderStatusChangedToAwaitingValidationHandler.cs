namespace Products.Application.IntegrationEventsHandler;

public class OrderStatusChangedToAwaitingValidationHandler : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidation>
{
    private readonly IProductRepository _repo;
    private readonly IEventBus _bus;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationHandler> _logger;

    public OrderStatusChangedToAwaitingValidationHandler(IProductRepository repo, IEventBus bus, ILogger<OrderStatusChangedToAwaitingValidationHandler> logger)
    {
        _repo = repo;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToAwaitingValidation> context)
    {
        var msg = context.Message;

        _logger.LogInformation($"OrderStatusChangedToAwaitingValidationHandler received msg for orderId:{msg.OrderId}");


        var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

        foreach (var orderStockItem in msg.OrderStockItems)
        {
            var catalogItem = await _repo.GetByIdAsync(orderStockItem.ProductId);
            var hasStock = catalogItem.Quantity >= orderStockItem.Units;
            var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

            confirmedOrderStockItems.Add(confirmedOrderStockItem);
        }


        if (confirmedOrderStockItems.Any(c => !c.HasStock))
        {
            var orderStockRejected = new OrderStockRejected(msg.OrderId, confirmedOrderStockItems);
            await _bus.PublishAsync(orderStockRejected);
            _logger.LogInformation($"OrderStatusChangedToAwaitingValidationHandler published OrderStockRejected for orderId:{msg.OrderId}");
        }
        else
        {
            var orderStockConfirmed = new OrderStockConfirmed(msg.OrderId);
            await _bus.PublishAsync(orderStockConfirmed);

            _logger.LogInformation($"OrderStatusChangedToAwaitingValidationHandler published OrderStockConfirmed for orderId:{msg.OrderId}");
        }

    }
}
