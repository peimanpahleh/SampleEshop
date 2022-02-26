using EventBus.IntegrationEvents.OrderSaga;
using EventBus.IntegrationEvents.Payments;
using EventBus.IntegrationEvents.Products;

namespace Orders.Saga.IntegrationTests;

public class OrderCanceledTests : StateMachineTestFixture<OrderProccesingStateMachine, OrderState>
{
    [Test]
    public async Task Should_remains_canceled_state()
    {
        var orderStarted = GetOrderStarted();
        var orderId = Guid.Parse(orderStarted.OrderId);

        await SubmitOrder(orderStarted);
        await ConfirmProduct(orderStarted.OrderId);


        // payment - failed

        await FailedPayment(orderStarted.OrderId);

        // now state is canceled
        await TestHarness.Bus.Publish(orderStarted);

        // submit
        Assert.IsTrue(await TestHarness.Consumed.Any<OrderStarted>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderStarted>(), "Message not consumed by saga");
        Assert.That(await SagaHarness.Created.Any(x => x.CorrelationId == orderId));
        var instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Canceled);
        Assert.IsNotNull(instance, "Saga instance not found");


        // confirm
        var orderStockConfirmed = new OrderStockConfirmed(orderId.ToString());
        await TestHarness.Bus.Publish(orderStockConfirmed);
        Assert.IsTrue(await TestHarness.Consumed.Any<OrderStockConfirmed>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderStockConfirmed>(), "Message not consumed by saga");
        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToStockConfirmed>(), "Message not published");
        instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Canceled);
        Assert.IsNotNull(instance, "Saga instance not found");


        // success-payment
        var orderPaymentSucceeded = new OrderPaymentSucceeded(orderId.ToString());
        await TestHarness.Bus.Publish(orderPaymentSucceeded);
        Assert.IsTrue(await TestHarness.Consumed.Any<OrderPaymentSucceeded>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderPaymentSucceeded>(), "Message not consumed by saga");
        Assert.IsFalse(await TestHarness.Published.Any<OrderStatusChangedToPaid>(), "Message not published");
        instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Canceled);
        Assert.IsNotNull(instance, "Saga instance not found");


        // failed-payment
        var orderPaymentFailed = new OrderPaymentFailed(orderId.ToString());
        await TestHarness.Bus.Publish(orderPaymentFailed);
        Assert.IsTrue(await TestHarness.Consumed.Any<OrderPaymentFailed>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderPaymentFailed>(), "Message not consumed by saga");
        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToCancelled>(), "Message not published");
        instance = SagaHarness.Created.ContainsInState(orderId, Machine, Machine.Canceled);
        Assert.IsNotNull(instance, "Saga instance not found");

    }

    private async Task SubmitOrder(OrderStarted orderStarted)
    {
        var orderId = Guid.Parse(orderStarted.OrderId);

        await TestHarness.Bus.Publish(orderStarted);

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
    }
    private async Task ConfirmProduct(string orderId)
    {
        var orderStockConfirmed = new OrderStockConfirmed(orderId);
        await TestHarness.Bus.Publish(orderStockConfirmed);

        Assert.IsTrue(await TestHarness.Consumed.Any<OrderStockConfirmed>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderStockConfirmed>(), "Message not consumed by saga");
        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToStockConfirmed>(), "Message not published");

        await AdvanceSystemTime(TimeSpan.FromSeconds(2));

        var instance = SagaHarness.Created.ContainsInState(Guid.Parse(orderId), Machine, Machine.Validated);
        Assert.IsNotNull(instance, "Saga instance not found");

    }

    private async Task FailedPayment(string orderId)
    {
        var orderPaymentFailed = new OrderPaymentFailed(orderId);
        await TestHarness.Bus.Publish(orderPaymentFailed);

        Assert.IsTrue(await TestHarness.Consumed.Any<OrderPaymentFailed>(), "Message not consumed");
        Assert.IsTrue(await SagaHarness.Consumed.Any<OrderPaymentFailed>(), "Message not consumed by saga");
        Assert.IsTrue(await TestHarness.Published.Any<OrderStatusChangedToCancelled>(), "Message not published");

        await AdvanceSystemTime(TimeSpan.FromSeconds(2));

        var instance = SagaHarness.Created.ContainsInState(Guid.Parse(orderId), Machine, Machine.Canceled);
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

