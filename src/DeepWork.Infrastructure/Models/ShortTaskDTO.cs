using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;
using SQLite;

namespace DeepWork.Infrastructure.Models;

[Table("short-tasks")]
public class ShortTaskDTO : IAggregateRoot
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(ShortTask.NameLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(ShortTask.DescriptionLength)]
    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Indexed]
    public int LongTaskId { get; set; }
}
