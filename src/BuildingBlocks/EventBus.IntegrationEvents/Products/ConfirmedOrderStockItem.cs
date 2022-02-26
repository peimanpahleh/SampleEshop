namespace EventBus.IntegrationEvents.Products;

public record ConfirmedOrderStockItem
{
    public string ProductId { get; }
    public bool HasStock { get; }

    public ConfirmedOrderStockItem(string productId, bool hasStock)
    {
        ProductId = productId;
        HasStock = hasStock;
    }
}