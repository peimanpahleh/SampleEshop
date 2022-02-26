
namespace Orders.UnitTests.Orders;

public class OrderItemTests
{
    [Test]
    public void Should_create_order_item()
    {
        var itemId = Guid.NewGuid().ToString();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 10m;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 10;
        var imageId = Guid.NewGuid().ToString();

        OrderItem orderItem = new(itemId, productId, productName, unitPrice, unitPriceId, imageId, quantity);

        orderItem.Should().NotBeNull();
        orderItem.Id.Should().Be(itemId);
        orderItem.ProductId.Should().Be(productId);
        orderItem.ProductName.Should().Be(productName);
        orderItem.UnitPrice.Should().Be(unitPrice);
        orderItem.UnitPriceId.Should().Be(unitPriceId);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.ImageId.Should().Be(imageId);

    }


    [Test]
    public void Should_change_quantity_of_item()
    {
        var item = GetOrderItem();

        var quantity = item.Quantity;
        var addQuantity = 5;
        var totalQuantity = quantity + addQuantity;

        item.AddUnits(addQuantity);

        item.Quantity.Should().Be(totalQuantity);
    }

    [Test]
    public void Should_return_total_price()
    {
        var item = GetOrderItem();

        var price = item.UnitPrice;
        var quantity = item.Quantity;

        var totalPrice = price * quantity;

        item.TotalPrice().Should().Be(totalPrice);
    }

    private OrderItem GetOrderItem()
    {
        var itemId = Guid.NewGuid().ToString();
        var productId = Guid.NewGuid().ToString();
        var productName = "p01";
        var unitPrice = 10m;
        var unitPriceId = Guid.NewGuid().ToString();
        var quantity = 10;
        var imageId = Guid.NewGuid().ToString();

        OrderItem orderItem = new(itemId, productId, productName, unitPrice, unitPriceId, imageId, quantity);
        return orderItem;
    }

}
