namespace EventBus.IntegrationEvents.OrderSaga;

public record OrderStatusChangedToCancelled(
            string OrderId,
            string BuyerId,
            string BuyerName
            ) : IntegerationEvent;


