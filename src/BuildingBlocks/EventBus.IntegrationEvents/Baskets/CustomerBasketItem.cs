namespace EventBus.IntegrationEvents.Baskets;

public record CustomerBasketItem(
    string Id,
    string ProductId,
    string ProductName,
    decimal UnitPrice,
    string UnitPriceId,
    int Quantity,
    string ImageId);