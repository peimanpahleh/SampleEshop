namespace Orders.Domain.Orders;

public class OrderItem
{

    public string Id { get; private set; }
    public string ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string UnitPriceId { get; private set; }
    public int Quantity { get; private set; }
    public string ImageId { get; private set; }


    protected OrderItem() 
    {

    }

    public OrderItem(string id,
        string productId,
        string productName,
        decimal unitPrice,
        string unitPriceId,
        string imageId,
        int quantity = 1)
    {
        if (quantity <= 0)
        {
            throw new OrderingDomainException("Invalid number of units");
        }

        /*if ((unitPrice * quantity) < discount)
        {
            throw new OrderingDomainException("The total of order item is lower than applied discount");
        }*/

        Id = id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        UnitPriceId = unitPriceId;
        Quantity = quantity;
        ImageId = imageId;
    }

    public void AddUnits(int quantity)
    {
        if (quantity < 0)
        {
            throw new OrderingDomainException("Invalid units");
        }

        Quantity += quantity;
    }

    public decimal TotalPrice()
    {
        return UnitPrice * Quantity;
    }


}
