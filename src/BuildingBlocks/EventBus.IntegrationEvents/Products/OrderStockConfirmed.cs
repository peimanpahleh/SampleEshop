namespace EventBus.IntegrationEvents.Products;

public record OrderStockConfirmed(string OrderId) : IntegerationEvent;
