using DeepWork.Domain.Common;
using SQLite;

namespace DeepWork.Domain.Entities;

[Table("short-tasks")]
public class ShortTask : HasDomainEventBase
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Indexed]
    public int LongTaskId { get; set; }
}
