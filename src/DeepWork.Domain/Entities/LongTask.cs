using DeepWork.Domain.Common;

namespace DeepWork.Domain.Entities;

public class LongTask : HasDomainEventBase
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
