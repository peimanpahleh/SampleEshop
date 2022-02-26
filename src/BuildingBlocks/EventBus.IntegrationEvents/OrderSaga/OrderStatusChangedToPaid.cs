namespace EventBus.IntegrationEvents.OrderSaga;

public record OrderStatusChangedToPaid(
            string OrderId,
            string BuyerId,
            string BuyerName,
            IEnumerable<OrderStockItem> OrderStockItems) : IntegerationEvent;


