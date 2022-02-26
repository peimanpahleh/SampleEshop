
using Orders.Application.Models.Dto;

namespace Orders.IntegrationTests.Orders.Commands;

public class SetStockConfirmedOrderStatusCommandTests : IClassFixture<MongoFixture>
{
    private readonly IOrderRepository _orderRepo;
    private readonly IUnitOfWork _unitOfWork;

    public SetStockConfirmedOrderStatusCommandTests(MongoFixture fixture)
    {
        _orderRepo = fixture.ServiceProvider.GetRequiredService<IOrderRepository>();
        _unitOfWork = fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();

    }

    [Fact]
    public async Task Should_change_order_status_to_stock_confirm()
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
            var res = await _unitOfWork.SaveChangesAsync();

            var command = new SetAwaitingValidationOrderStatusCommand(order.Id);
            var handler = new SetAwaitingValidationOrderStatusCommandHandler(_orderRepo,_unitOfWork);
            await handler.Handle(command, CancellationToken.None);

            var find = await _orderRepo.GetByIdAsync(order.Id);

            find.Should().NotBeNull();
            find.OrderStatus.Should().Be(OrderStatus.AwaitingValidation);

            var command2 = new SetStockConfirmedOrderStatusCommand(order.Id);
            var handler2 = new SetStockConfirmedOrderStatusCommandHandler(_orderRepo, _unitOfWork);
            await handler2.Handle(command2, CancellationToken.None);

            var find2 = await _orderRepo.GetByIdAsync(order.Id);
            find2.Should().NotBeNull();
            find2.OrderStatus.Should().Be(OrderStatus.StockConfirmed);

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
