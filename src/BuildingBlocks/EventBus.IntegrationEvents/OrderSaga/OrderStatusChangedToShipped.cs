namespace EventBus.IntegrationEvents.OrderSaga;

public record OrderStatusChangedToShipped(
            string OrderId,
            string OrderStatus,
            string BuyerName) : IntegerationEvent;


