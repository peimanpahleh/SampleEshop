
using Orders.Application.Models.Dto;

namespace Orders.IntegrationTests.Orders.Commands;

public class SetAwaitingValidationOrderStatusCommandTests : IClassFixture<MongoFixture>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IUnitOfWork _unitOfWork;


    public SetAwaitingValidationOrderStatusCommandTests(MongoFixture fixture)
    {
        _orderRepo = fixture.ServiceProvider.GetRequiredService<IOrderRepository>();
        _unitOfWork = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task Should_change_order_status_to_awaiting_validation()
    {
        // masstransit 
        var harness = new InMemoryTestHarness();
        await harness.Start();
        try
        {
          List<OrderItemDto> orderItemsDto = new();
            orderItemsDto.Add(GetOrderItem());

            var userId = Guid.NewGuid().ToString();
            var userName = "user01";

            // map OrderItemDto to OrderItem
            var orderItems = orderItemsDto.Select(x => new OrderItem(x.Id,
                x.ProductId,
                x.ProductName,
                x.UnitPrice,
                x.UnitPriceId,
                x.ImageId,
                x.Quantity)).ToList();


            var order = new Order(userId, userName, orderItems);

            _orderRepo.Add(order);
            await _unitOfWork.SaveChangesAsync();

            var command = new SetAwaitingValidationOrderStatusCommand(order.Id);
            var handler = new SetAwaitingValidationOrderStatusCommandHandler(_orderRepo,_unitOfWork);
            await handler.Handle(command,CancellationToken.None);

            var find = await _orderRepo.GetByIdAsync(order.Id);

            find.Should().NotBeNull();
            find.OrderStatus.Should().Be(OrderStatus.AwaitingValidation);
            

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
