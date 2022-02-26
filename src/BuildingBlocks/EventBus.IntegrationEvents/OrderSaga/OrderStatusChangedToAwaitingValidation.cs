namespace EventBus.IntegrationEvents.OrderSaga;

public record OrderStatusChangedToAwaitingValidation(
            string OrderId,
            IEnumerable<OrderStockItem> OrderStockItems) : IntegerationEvent;


