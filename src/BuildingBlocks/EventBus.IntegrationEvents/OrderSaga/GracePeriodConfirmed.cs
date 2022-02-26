namespace EventBus.IntegrationEvents.OrderSaga;

public record GracePeriodConfirmed(string OrderId) : IntegerationEvent;
