using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;
using SQLite;

namespace DeepWork.Infrastructure.Models;

[Table("long-tasks")]
public class LongTaskDTO : IAggregateRoot
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(LongTask.NameLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(LongTask.DescriptionLength)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
