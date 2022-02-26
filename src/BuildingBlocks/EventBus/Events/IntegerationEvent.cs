namespace EventBus.Events;

public record IntegerationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreationDate { get; init; } = DateTime.UtcNow;
}
