namespace EventBus.IntegrationEvents.OrderSaga;

public record OrderStatusChangedToSubmitted(
            string OrderId,
            string OrderStatus,
            string BuyerName) : IntegerationEvent;


