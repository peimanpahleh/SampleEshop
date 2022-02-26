namespace EventBus.IntegrationEvents.Baskets;

public record UserCheckoutAccepted(
        string UserId,
        string UserName,
        CustomerBasket Basket) : IntegerationEvent;