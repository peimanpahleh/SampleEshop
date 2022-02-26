
namespace EventBus.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IConsumer<TIntegrationEvent>
 where TIntegrationEvent : IntegerationEvent
{
}
