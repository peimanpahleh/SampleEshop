namespace EventBus.IntegrationEvents.Products;

public record ProductPriceChanged(
        string ProductId,
        decimal OldPrice,
        decimal NewPrice,
        string NewPriceId) : IntegerationEvent;
