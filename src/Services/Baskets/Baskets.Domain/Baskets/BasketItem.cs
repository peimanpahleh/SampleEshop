namespace Baskets.Domain.Baskets;

public class BasketItem
{

    public Guid Id { get; private set; }

    public string ProductId { get; private set; }

    public string ProductName { get; private set; }

    public decimal UnitPrice { get; private set; }

    public string UnitPriceId { get; private set; }

    public int Quantity { get; private set; }

    public string ImageId { get; private set; }


    protected BasketItem()
    {

    }

    public BasketItem(Guid id, string productId, string productName, decimal unitPrice, string unitPriceId, int quantity, string imageId)
    {
        if (id == Guid.Empty)
            throw new InvalidBasketItemException($"ItemId:{id} not valid");

        if (string.IsNullOrEmpty(productId))
            throw new InvalidBasketItemException($"ProductId cannot be null");

        if (string.IsNullOrEmpty(productName))
            throw new InvalidBasketItemException($"ProductName cannot be null");

        if (string.IsNullOrEmpty(unitPriceId))
            throw new InvalidBasketItemException($"UnitPriceId cannot be null");

        if (string.IsNullOrEmpty(imageId))
            throw new InvalidBasketItemException($"ImageId cannot be null");

        if (unitPrice <= 0)
            throw new InvalidBasketItemException($"UnitPrice not valid");

        if (quantity <= 0)
            throw new InvalidBasketItemException($"Quantity not valid");

        Id = id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        UnitPriceId = unitPriceId;
        Quantity = quantity;
        ImageId = imageId;
    }

    public void ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidBasketItemException($"Quantity not valid");

        if (Quantity == quantity)
            return;

        Quantity = quantity;
    }

    public void ChangePrice(decimal price,string priceId)
    {
        if (price <= 0)
            throw new InvalidBasketItemException($"Quantity not valid");

        UnitPrice = price;
        UnitPriceId = priceId;
    }
}