
using Orders.Application.Models.Dto;

namespace Orders.IntegrationTests.Orders.Commands;

public class CreateOrderCommandTests : IClassFixture<MongoFixture>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public CreateOrderCommandTests(MongoFixture fixture)
    {
        _orderRepo = fixture.ServiceProvider.GetRequiredService<IOrderRepository>();
        _unitOfWork = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _eventBus = Mock.Of<IEventBus>();
    }

    [Fact]
    public async Task Should_create_order()
    {
        // masstransit 
        var harness = new InMemoryTestHarness();
        await harness.Start();

        try
        {
            List<OrderItemDto> orderItems = new();
            orderItems.Add(GetOrderItem());

            var userId = Guid.NewGuid().ToString();
            var userName = "user01";

            var command = new CreateOrderCommand(userId, userName, orderItems);
            var handler = new CreateOrderCommandHandler(_orderRepo,_unitOfWork,_eventBus);
            var res = await handler.Handle(command, CancellationToken.None);
            res.Should().BeTrue();
        }
        finally
        {
            await harness.Stop();
        }
    }

    private OrderItemDto GetOrderItem()
    {
        var id = Guid.NewGuid().ToString();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        decimal unitPrice = 20;
        var unitPriceId = Guid.NewGuid().ToString();
        int quantity = 2;
        string imageId = Guid.NewGuid().ToString();

        return new OrderItemDto(id, productId, productName, unitPrice, unitPriceId, quantity, imageId);
    }

}
