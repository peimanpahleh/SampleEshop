namespace Products.Application.IntegrationEventsHandler;

public class OrderStatusChangedToPaidHandler : IIntegrationEventHandler<OrderStatusChangedToPaid>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationHandler> _logger;

    public OrderStatusChangedToPaidHandler(IProductRepository repo,
        ILogger<OrderStatusChangedToAwaitingValidationHandler> logger, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToPaid> context)
    {
        var msg = context.Message;

        _logger.LogInformation($"OrderStatusChangedToPaidHandler received msg orderId:{msg.OrderId}");

        //we're not blocking stock/inventory
        foreach (var orderStockItem in msg.OrderStockItems)
        {
            var catalogItem = await _repo.GetByIdAsync(orderStockItem.ProductId);

            catalogItem.RemoveStock(orderStockItem.Units);

            // update mongo
             _repo.Update(catalogItem);
            await _unitOfWork.SaveChangesAsync();

        }

    }
}
