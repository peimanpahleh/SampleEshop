namespace Orders.Domain.Events;

public record OrderStartedDomainEvent(string OrderId,string UserId,string UserName) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
