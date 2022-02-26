
namespace Baskets.IntegrationTests.Queries;

public class GetBasketQueryTests : IClassFixture<RedisFixture>
{
    private readonly IBasketRepository _repo;
    private readonly ITestOutputHelper _output;

    public GetBasketQueryTests(RedisFixture fixture, ITestOutputHelper output)
    {
        _repo = fixture.ServiceProvider.GetRequiredService<IBasketRepository>();
        _output = output;
    }

    /*[Fact]
    public async Task Should_return_basket()
    {
        var basket = GetBasket();
        await _repo.UpdateBasketAsync(basket);

        var query = new GetBasketQuery(basket.BuyerId);
        var queryHandler = new GetBasketQueryHandler(_repo);

        var res =  await queryHandler.Handle(query, CancellationToken.None);

        res.Should().NotBeNull();
        res.BuyerId.Should().BeEquivalentTo(query.CustomerId);

    }

    private Basket GetBasket()
    {
        var buyerId = Guid.NewGuid().ToString();
        _output.WriteLine($"BuyerId: {buyerId}");


        List<BasketItem> basketItems = new();

        basketItems.Add(GetBasketItem());

        var basket = new Basket()
        {
            BuyerId = buyerId,
            BasketItems = basketItems
        };

        return basket;
    }

    private BasketItem GetBasketItem()
    {
        var basketItem = new BasketItem()
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = Guid.NewGuid().ToString(),
            ProductName = "test",
            Quantity = 2,
            UnitPrice = 13
        };

        return basketItem;
    }*/

}
