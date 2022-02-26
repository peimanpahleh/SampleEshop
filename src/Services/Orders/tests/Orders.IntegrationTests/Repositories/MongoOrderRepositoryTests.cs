namespace Orders.IntegrationTests.Repositories;

public class MongoOrderRepositoryTests : IClassFixture<MongoFixture>
{
    private readonly IOrderRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    public MongoOrderRepositoryTests(MongoFixture fixture)
    {
        _repo = fixture.ServiceProvider.GetRequiredService<IOrderRepository>();
        _unitOfWork = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task Should_add_order()
    {
        var order = GetOrder();

        _repo.Add(order);
        await _unitOfWork.SaveChangesAsync();

        var res =  await _repo.GetByIdAsync(order.Id);
        res.Should().NotBeNull();

        res.BuyerId.Should().NotBeNull();
    }

    private Order GetOrder()
    {
        //var orderId = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid().ToString();
        var userName = "user01";

        List<OrderItem> orderItems = new();
        var itemId = Guid.NewGuid().ToString();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 10m;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 10;
        var imageId = Guid.NewGuid().ToString();
        OrderItem orderItem = new(itemId, productId, productName, unitPrice, unitPriceId, imageId, quantity);
        orderItems.Add(orderItem);

        Order order = new(userId, userName, orderItems);
        return order;
    }
}
