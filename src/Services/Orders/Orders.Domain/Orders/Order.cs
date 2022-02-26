using Orders.Domain.Events;

namespace Orders.Domain.Orders;

public class Order : Entity , IAggregateRoot
{
    public string BuyerId { get; private set; }

    public DateTime OrderDate { get; private set; }
    public OrderStatus OrderStatus { get; set; }
    public string Description { get; private set; }

    public List<OrderItem> OrderItems { get; private set; } = new();

    public Order(string userId, string userName, List<OrderItem> orderItems)
    {
        OrderStatus = OrderStatus.Submitted;
        OrderItems = orderItems;
        OrderDate = DateTime.UtcNow;

        IncreaseVersion();

        AddOrderStartedDomainEvent(Id,userId,userName);

    }

    public void SetBuyerId(string id)
    {
        BuyerId = id;

        IncreaseVersion();
    }

    public decimal GetTotalPrice()
    {
       return OrderItems.Select(x => x.TotalPrice()).Sum();
    }

    public void SetAwaitingValidationStatus()
    {
        if (OrderStatus.Id == OrderStatus.Submitted.Id)
        {
            OrderStatus = OrderStatus.AwaitingValidation;
        }

        IncreaseVersion();

    }

    public void SetStockConfirmedStatus()
    {
        if (OrderStatus.Id == OrderStatus.AwaitingValidation.Id)
        {
            OrderStatus = OrderStatus.StockConfirmed;
            Description = "All the items were confirmed with available stock.";
        }

        IncreaseVersion();

    }

    public void SetPaidStatus()
    {
        if (OrderStatus.Id == OrderStatus.StockConfirmed.Id)
        {
            OrderStatus = OrderStatus.Paid;
            Description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
        }

        IncreaseVersion();

    }

    public void SetCancelledStatus()
    {
        if (OrderStatus.Id == OrderStatus.Paid.Id ||
            OrderStatus.Id == OrderStatus.Shipped.Id)
        {
            //StatusChangeException(OrderStatus.Cancelled);
        }

        OrderStatus = OrderStatus.Cancelled;
        Description = $"The order was cancelled.";

        IncreaseVersion();

    }

    // events

    private void AddOrderStartedDomainEvent(string orderId,string userId, string userName)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(orderId,userId,userName);

        AddDomainEvent(orderStartedDomainEvent);
    }
}
