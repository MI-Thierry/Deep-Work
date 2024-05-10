namespace DeepWork.Domain.Common;

public class EntityBase : HasDomainEventBase
{
    public int Id { get; set; }
}

public class EntityBase<TIn> : HasDomainEventBase
{
    public TIn Id { get; set; }
}