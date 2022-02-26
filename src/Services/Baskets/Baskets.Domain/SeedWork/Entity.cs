﻿using System.Text.Json.Serialization;

namespace Baskets.Domain.SeedWork;


public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    [JsonIgnore]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    public Guid Id { get; protected set; }

    public int Version { get; protected set; } = 0;

    protected Entity()
    {
    }


    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void IncreaseVersion()
    {
        Version++;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Entity other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }

    private Type GetRealType()
    {
        Type type = GetType();

        if (type.ToString().Contains("Castle.Proxies."))
            return type.BaseType;

        return type;
    }
}
