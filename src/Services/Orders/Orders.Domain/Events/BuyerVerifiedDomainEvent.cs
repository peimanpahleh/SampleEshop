namespace Orders.Domain.Events;

public record BuyerVerifiedDomainEvent(string BuyerId, string OrderId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
