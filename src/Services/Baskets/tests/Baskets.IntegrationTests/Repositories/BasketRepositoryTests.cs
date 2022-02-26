using System.Text.Json;

namespace Baskets.IntegrationTests.Repositories;

public class BasketRepositoryTests : IClassFixture<RedisFixture>
{
    private readonly IBasketRepository _repo;
    private readonly ITestOutputHelper _output;

    public BasketRepositoryTests(RedisFixture fixture, ITestOutputHelper output)
    {
        _repo = fixture.ServiceProvider.GetRequiredService<IBasketRepository>();
        _output = output;
    }

    [Fact]
    public async Task Should_add_or_update_basket()
    {
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        _output.WriteLine($"BuyerId: {buyerId}");

        // basketItem
        var ItemId = Guid.NewGuid();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();


        var basket = new Basket(buyerId, buyerName);

        basket.Version.Should().Be(1);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId, productId, productName, unitPrice, unitPriceId, quantity, imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        await _repo.UpdateBasketAsync(basket);

        var res = await _repo.GetBasketAsync(buyerId);

        res.Should().NotBeNull();
        res.Id.Should().Be(buyerId);
        res.BasketItems.Count.Should().Be(1);
        res.BasketItems.First().Quantity.Should().Be(1);
        res.Version.Should().Be(2);

        // update Quantity
        res.ChangeQuantity(ItemId,2);
        await _repo.UpdateBasketAsync(res);

        var res1 = await _repo.GetBasketAsync(buyerId);

        res1.Should().NotBeNull();
        res1.Id.Should().Be(buyerId);
        res1.BasketItems.Count.Should().Be(1);

        // check quantity updated or not
        res1.BasketItems.First().Quantity.Should().Be(2);

        res1.Version.Should().Be(3);

    }

    [Fact]
    public async Task Should_delete_basket()
    {
        var buyerId = Guid.NewGuid();
        var buyerName = "test buyer";

        _output.WriteLine($"BuyerId: {buyerId}");

        // basketItem
        var ItemId = Guid.NewGuid();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 5;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 1;
        var imageId = Guid.NewGuid().ToString();

        var basket = new Basket(buyerId, buyerName);

        List<BasketItem> basketItems = new();
        var basketItem = new BasketItem(ItemId, productId, productName, unitPrice, unitPriceId, quantity, imageId);
        basketItems.Add(basketItem);
        basket.AddItems(basketItems);

        await _repo.UpdateBasketAsync(basket);

        var res = await _repo.GetBasketAsync(buyerId);

        res.Should().NotBeNull();
        res.Id.Should().Be(buyerId);
        res.BasketItems.Count.Should().Be(1);

        res.BasketItems.First().Quantity.Should().Be(1);

        // update Quantity
        basket.ChangeQuantity(ItemId, 2);
        await _repo.UpdateBasketAsync(basket);

        res = await _repo.GetBasketAsync(buyerId);

        res.Should().NotBeNull();
        res.Id.Should().Be(buyerId);
        res.BasketItems.Count.Should().Be(1);

        // check quantity updated or not
        res.BasketItems.First().Quantity.Should().Be(2);


        await _repo.DeleteBasketAsync(buyerId);

        res = await _repo.GetBasketAsync(buyerId);
        
        res.Should().BeNull();
    }
}
