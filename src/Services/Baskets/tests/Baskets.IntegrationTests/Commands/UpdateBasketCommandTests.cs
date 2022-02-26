using Baskets.Application.Models.Dto.User;

namespace Baskets.IntegrationTests.Commands;

public class UpdateBasketCommandTests : IClassFixture<RedisFixture>
{
    private readonly IBasketRepository _repo;
    private readonly ITestOutputHelper _output;

    public UpdateBasketCommandTests(RedisFixture fixture, ITestOutputHelper output)
    {
        _repo = fixture.ServiceProvider.GetRequiredService<IBasketRepository>();
        _output = output;
    }

    [Fact]
    public async Task Should_create_or_update_basket()
    {
        //var basket = GetBasket();
        
       /* var basketDto = new BasketDto(basket.BuyerId,
        basket.BasketItems.Select(x => new BasketItemDto(x.Id,x.ProductId,x.ProductName,
                    x.UnitPrice,x.Quantity)).ToList());

        var command = new UpdateBasketCommand(basketDto);
        var commandHandler = new UpdateBasketCommandHandler(_repo);

        var res =  await commandHandler.Handle(command, CancellationToken.None);

        res.Should().Be(basket.BuyerId);*/

        // check for publish event...

    }

    /*private Basket GetBasket()
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
