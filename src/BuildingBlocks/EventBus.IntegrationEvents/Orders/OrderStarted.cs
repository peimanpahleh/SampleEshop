using EventBus.IntegrationEvents.Baskets;
using EventBus.IntegrationEvents.OrderSaga;

namespace EventBus.IntegrationEvents.Orders;

public record OrderStarted(
    string OrderId,
    string BuyerId,
    string BuyerName,
    IEnumerable<CustomerBasketItem> OrderItems) : IntegerationEvent;
