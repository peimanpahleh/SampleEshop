namespace EventBus.IntegrationEvents.Payments;

public record OrderPaymentFailed(string OrderId) : IntegerationEvent;
