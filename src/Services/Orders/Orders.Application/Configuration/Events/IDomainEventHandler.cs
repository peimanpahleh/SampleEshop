namespace Orders.Application.Configuration.Events;

public interface IDomainEventHandler<T> : INotificationHandler<T> where T : IDomainEvent
{
    
}
