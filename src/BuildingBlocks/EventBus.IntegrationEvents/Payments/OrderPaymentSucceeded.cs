namespace EventBus.IntegrationEvents.Payments;

public record OrderPaymentSucceeded(string OrderId) : IntegerationEvent;
