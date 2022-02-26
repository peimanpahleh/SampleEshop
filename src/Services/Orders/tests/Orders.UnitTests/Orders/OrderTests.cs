
namespace Orders.UnitTests.Orders;

public class OrderTests
{
    [Test]
    public void Should_create_order()
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
        OrderItem orderItem = new(itemId,productId,productName,unitPrice,unitPriceId,imageId,quantity);

        orderItems.Add(orderItem);


        Order order = new(userId,userName,orderItems);

        order.Should().NotBeNull();
        order.Id.Should().NotBeNull();
        order.BuyerId.Should().BeNull();
        order.OrderItems.Count.Should().Be(1);

    }

    [Test]
    public void Should_set_buyerId()
    {
        var buyerId = Guid.NewGuid().ToString();
        var order = GetOrder();
        order.BuyerId.Should().BeNull();
        order.SetBuyerId(buyerId);
        order.BuyerId.Should().Be(buyerId);
    }

    [Test]
    public void When_order_created_should_raises_an_event()
    {
        var order = GetOrder();
        order.DomainEvents.Count.Should().Be(1);
    }

    [Test]
    public void Should_clear_events()
    {
        var order = GetOrder();
        order.DomainEvents.Count.Should().Be(1);
        order.ClearDomainEvents();
        order.DomainEvents.Count.Should().Be(0);
    }

    [Test]
    public void Should_return_total_price_for_single_item()
    {
        var order = GetOrder();

        var item = order.OrderItems.First();
        var totalPrice = item.UnitPrice * item.Quantity;

        order.GetTotalPrice().Should().Be(totalPrice);
    }

    [Test]
    public void Should_return_total_price()
    {
        var order = GetOrderItems(10);

        var totalPrice = order.OrderItems.Sum(x => x.UnitPrice * x.Quantity);
        
        order.GetTotalPrice().Should().Be(totalPrice); 
    }

    [Test]
    public void Should_change_order_status_to_AwaitingValidation()
    {
        var order = GetOrder();

        order.SetAwaitingValidationStatus();

        order.OrderStatus.Should().Be(OrderStatus.AwaitingValidation);
    }

    [Test]
    public void Should_change_order_status_to_StockConfirmed()
    {
        var order = GetOrder();

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();

        order.OrderStatus.Should().Be(OrderStatus.StockConfirmed);
    }

    [Test]
    public void Should_not_change_order_status_to_StockConfirmed_when_order_status_is_invalid()
    {
        var order = GetOrder();

        order.SetStockConfirmedStatus();

        order.OrderStatus.Should().Be(OrderStatus.Submitted);
    }

    [Test]
    public void Should_change_order_status_to_Paid()
    {
        var order = GetOrder();

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();

        order.OrderStatus.Should().Be(OrderStatus.Paid);
    }

    [Test]
    public void Should_not_change_order_status_to_Paid_when_order_status_is_invalid()
    {
        var order = GetOrder();

        order.SetPaidStatus();

        order.OrderStatus.Should().Be(OrderStatus.Submitted);
    }

    [Test]
    public void Should_change_order_status_to_Cancelled()
    {
        var order = GetOrder();

        order.SetCancelledStatus();

        order.OrderStatus.Should().Be(OrderStatus.Cancelled);
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

        Order order = new(userId,userName, orderItems);
        return order;
    }

    private Order GetOrderItems(int count)
    {
        var random = new Random();

        //var orderId = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid().ToString();
        var userName = "user01";

        List<OrderItem> orderItems = new();

        for (int i = 0; i < count; i++)
        {
            var itemId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var productName = $"p{i}";
            var unitPrice = random.Next(1,20);
            var unitPriceId = Guid.NewGuid().ToString();
            var quantity = random.Next(1,5);
            var imageId = Guid.NewGuid().ToString();
            OrderItem orderItem = new(itemId, productId, productName, unitPrice, unitPriceId, imageId, quantity);
            orderItems.Add(orderItem);
        }

        Order order = new(userId,userName, orderItems);
        return order;
    }
}
