namespace EventBus.IntegrationEvents.OrderSaga;

public record OrderStatusChangedToStockConfirmed(
            string OrderId,
            string BuyerId,
            string BuyerName
    ) : IntegerationEvent;


