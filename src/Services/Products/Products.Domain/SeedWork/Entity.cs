namespace Products.Domain.SeedWork;


public abstract class Entity
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public int Version { get; private set; } = 0;

    private List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    protected void IncreaseVersion()
    {
        Version++;
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}



