using EventBus.IntegrationEvents.OrderSaga;
using EventBus.IntegrationEvents.Payments;

namespace Orders.Saga.IntegrationTests;

public class OrderSubmittedTests : StateMachineTestFixture<OrderProccesingStateMachine, OrderState>
{
    

    [Test]
    public async Task Should_create_a_saga_instance_and_publish_OrderStatusChangedToAwaitingValidation()
    {
        var orderStarted = GetOrderStarted();
        var orderId = Guid.Parse(orderStarted.OrderId);

        await TestHarness.Bus.Publish(orderStarted);

        Assert.IsTrue(await TestHarness.Consumed.Any<OrderStarted>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderStarted>(), "Message not consumed by saga");
        Assert.That(await SagaHarness.Created.Any(x => x.CorrelationId == orderId));
        var instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Submitted);
        Assert.IsNotNull(instance, "Saga instance not found");
        var existsId = await SagaHarness.Exists(orderId, x => x.Submitted);
        Assert.IsTrue(existsId.HasValue, "Saga did not exist");

        await AdvanceSystemTime(TimeSpan.FromSeconds(5));

        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToAwaitingValidation>(), "Message not published");


        instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.AwaitingStockValidation);
        Assert.IsNotNull(instance, "Saga instance not found");
    }


    // When published OrderStarted, Should_create_a_saga_instance
    // When published OrderStarted, should publish OrderStatusChangedToAwaitingValidation after 1 sec
    // When published OrderStarted, should ignore success payment - OrderPaymentSucceeded


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

