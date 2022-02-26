namespace Baskets.Domain.Baskets;

public class Basket : Entity, IAggregateRoot
{
    public string BuyerName { get; private set; }

    private List<BasketItem> _basketItems = new();

    public IReadOnlyList<BasketItem> BasketItems => _basketItems.ToList();

    protected Basket()
    {

    }

    public Basket(Guid buyerId, string buyerName)
    {
        if (buyerId == Guid.Empty)
            throw new InvalidBasketException($"buyerId:{buyerId} not valid");

        if (string.IsNullOrEmpty(buyerName))
            throw new InvalidBasketException($"buyerName cannot be null");

        Id = buyerId;
        BuyerName = buyerName;

        IncreaseVersion();

    }

    public void AddItems(List<BasketItem> items)
    {
        _basketItems.AddRange(items);

        IncreaseVersion();

    }

    public void ChangeQuantity(Guid id, int quantity)
    {
        var item = _basketItems.Where(x => x.Id == id).FirstOrDefault();
        if (item == null)
            throw new InvalidBasketException($"Item:{id} not found");

        if (item.Quantity == quantity)
            return;

        IncreaseVersion();

        item.ChangeQuantity(quantity);

    }

    public void ChangePrice(string productId,decimal price,string priceId)
    {
        var item = _basketItems.Where(x => x.ProductId == productId).FirstOrDefault();
        if (item == null)
            return;

        item.ChangePrice(price, priceId);

        IncreaseVersion();
    }

    public void RemoveItem(Guid id)
    {
        var item = _basketItems.Where(x => x.Id == id).FirstOrDefault();
        if (item == null)
            return;

        _basketItems.Remove(item);

        IncreaseVersion();
    }

    public void SetVersion(int version)
    {
        Version = version;
    }
}