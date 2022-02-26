namespace Baskets.UnitTests.Baskets;

public class BasketTests
{
    [Fact]
    public void Should_create_basket_object()
    {
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        var basket = new Basket(buyerId, buyerName);

        basket.Id.Should().Be(buyerId);
        basket.BuyerName.Should().Be(buyerName);
    }


    [Fact]
    public void Should_throw_an_exception_when_buyer_id_not_valid()
    {
        Guid buyerId = Guid.Empty;
        var buyerName = "test buyer";

        Action basket = () => new Basket(buyerId, buyerName);

        basket.Should().Throw<InvalidBasketException>();
    }

    [Fact]
    public void Should_throw_an_exception_when_buyer_name_not_valid()
    {
        Guid buyerId = Guid.NewGuid();
        string buyerName = "";

        Action basket = () => new Basket(buyerId, buyerName);

        basket.Should().Throw<InvalidBasketException>();
    }

    [Fact]
    public void Should_add_basket_items()
    {
        // basket
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        // basketItem
        var ItemId = Guid.NewGuid();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();

        var basket = new Basket(buyerId, buyerName);

        basket.Id.Should().Be(buyerId);
        basket.BuyerName.Should().Be(buyerName);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId,productId,productName,unitPrice,unitPriceId,quantity,imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        basket.BasketItems.Count.Should().Be(1);
    }

    [Fact]
    public void Should_update_quantity()
    {
        // basket
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        // basketItem
        var ItemId = Guid.NewGuid();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();

        var basket = new Basket(buyerId, buyerName);

        basket.Id.Should().Be(buyerId);
        basket.BuyerName.Should().Be(buyerName);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId, productId, productName, unitPrice, unitPriceId, quantity, imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        basket.BasketItems.Count.Should().Be(1);

        basket.ChangeQuantity(ItemId,2);
        basket.BasketItems.First().Quantity.Should().Be(2);
    }

    [Fact]
    public void Should_remove_item()
    {
        // basket
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        // basketItem
        var ItemId = Guid.NewGuid();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();

        var basket = new Basket(buyerId, buyerName);

        basket.Id.Should().Be(buyerId);
        basket.BuyerName.Should().Be(buyerName);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId, productId, productName, unitPrice, unitPriceId, quantity, imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        basket.BasketItems.Count.Should().Be(1);

        basket.RemoveItem(ItemId);
        basket.BasketItems.Count.Should().Be(0);
    }
}
