namespace EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<T>(T integrationEvent) where T : IntegerationEvent;

}
