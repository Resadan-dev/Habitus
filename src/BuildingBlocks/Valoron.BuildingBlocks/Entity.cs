namespace Valoron.BuildingBlocks;

public abstract class Entity
{
    /// <summary>
    /// Unique identifier of the Entity.
    /// </summary>
    public Guid Id { get; protected set; }

    protected Entity() { }

    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// True when the Entity has no identity yet.
    /// </summary>
    public bool IsTransient() => Id == Guid.Empty;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (Entity)obj;

        if (IsTransient() || other.IsTransient())
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
        => !(left == right);

    private readonly List<object> _domainEvents = new();

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(object domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override int GetHashCode()
    {
        if (IsTransient())
            return base.GetHashCode();

        return HashCode.Combine(GetType(), Id);
    }
}
