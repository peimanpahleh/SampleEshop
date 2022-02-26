namespace EventBus.IntegrationEvents.Products;

public record OrderStockRejected(
        string OrderId,
        List<ConfirmedOrderStockItem> OrderStockItems) : IntegerationEvent;
