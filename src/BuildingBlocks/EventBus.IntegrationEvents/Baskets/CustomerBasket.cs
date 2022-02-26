namespace EventBus.IntegrationEvents.Baskets;

public class CustomerBasket
{
    public string BuyerId { get; set; }

    public List<CustomerBasketItem> Items { get; set; } = new();
}
