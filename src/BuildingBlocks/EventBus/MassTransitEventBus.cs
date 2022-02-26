namespace EventBus;

public class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MassTransitEventBus> _logger;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint, ILogger<MassTransitEventBus> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T integrationEvent) where T : IntegerationEvent
    {
        var topicName = integrationEvent.GetType().Name;

       /* _logger.LogInformation(
            "Publishing event {@Event} to {TopicName}",
            integrationEvent,
            topicName);*/

        await _publishEndpoint.Publish(integrationEvent);
    }

}
