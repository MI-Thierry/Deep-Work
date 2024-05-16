using DeepWork.Domain.Common;

namespace DeepWork.Domain.Entities;

public class ShortTask : HasDomainEventBase
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int LongTaskId { get; set; }
}
