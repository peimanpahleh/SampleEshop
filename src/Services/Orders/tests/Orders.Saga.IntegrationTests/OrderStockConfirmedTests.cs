using EventBus.IntegrationEvents.OrderSaga;
using EventBus.IntegrationEvents.Payments;
using EventBus.IntegrationEvents.Products;

namespace Orders.Saga.IntegrationTests;

// when OrderStockConfirmed then ....

public class OrderStockConfirmedTests : StateMachineTestFixture<OrderProccesingStateMachine, OrderState>
{
    [Test]
    public async Task Should_publish_OrderStatusChangedToStockConfirmed()
    {
        // publish orderStarted
        var orderStarted = GetOrderStarted();
        var orderId = Guid.Parse(orderStarted.OrderId);
        await TestHarness.Bus.Publish(orderStarted);

        // check for consumed and exists
        Assert.IsTrue(await TestHarness.Consumed.Any<OrderStarted>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderStarted>(), "Message not consumed by saga");
        Assert.That(await SagaHarness.Created.Any(x => x.CorrelationId == orderId));

        var instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Submitted);
        Assert.IsNotNull(instance, "Saga instance not found");

        var existsId = await SagaHarness.Exists(orderId, x => x.Submitted);
        Assert.IsTrue(existsId.HasValue, "Saga did not exist");

        await AdvanceSystemTime(TimeSpan.FromSeconds(2));


        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToAwaitingValidation>(), "Message not published");

        instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.AwaitingStockValidation);
        Assert.IsNotNull(instance, "Saga instance not found");

        // confirm product
        var orderStockConfirmed = new OrderStockConfirmed(orderStarted.OrderId);
        await TestHarness.Bus.Publish(orderStockConfirmed);

        Assert.IsTrue(await TestHarness.Consumed.Any<OrderStockConfirmed>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderStockConfirmed>(), "Message not consumed by saga");
        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToStockConfirmed>(), "Message not published");

        await AdvanceSystemTime(TimeSpan.FromSeconds(2));

        instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Validated);
        Assert.IsNotNull(instance, "Saga instance not found");
    }

    private OrderStarted GetOrderStarted()
    {
        List<CustomerBasketItem> orderItems = new();
        orderItems.Add(new CustomerBasketItem(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            "p01",
            3,
            Guid.NewGuid().ToString(),
            5,
            Guid.NewGuid().ToString()));

        var res = new OrderStarted(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            "b01",
            orderItems
            );

        return res;
    }
}

