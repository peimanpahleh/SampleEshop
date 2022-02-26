namespace Baskets.IntegrationTests.Commands;

public class DeleteBasketCommandTests : IClassFixture<RedisFixture>
{
    private readonly IBasketRepository _repo;
    private readonly ITestOutputHelper _output;

    public DeleteBasketCommandTests(RedisFixture fixture, ITestOutputHelper output)
    {
        _repo = fixture.ServiceProvider.GetRequiredService<IBasketRepository>();
        _output = output;
    }


    /*[Fact]
    public async Task Should_delete_basket()
    {
        var basket = GetBasket();
        await _repo.UpdateBasketAsync(basket);

        var command = new DeleteBasketCommand(basket.BuyerId);
        var commandHandler = new DeleteBasketCommandHandler(_repo);

        var res =  await commandHandler.Handle(command, CancellationToken.None);

        res.Should().BeTrue();

        //await Task.Delay(100);

        var find = await _repo.GetBasketAsync(basket.BuyerId);
        find.Should().BeNull();

        // check for publish event...

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
