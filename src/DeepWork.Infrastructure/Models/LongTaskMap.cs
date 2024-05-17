using DeepWork.Domain.Entities;
using SQLite;

namespace DeepWork.Infrastructure.Models;

[Table("long-tasks")]
public class LongTaskMap
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(LongTask.NameLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(LongTask.DescriptionLength)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public static explicit operator LongTaskMap?(LongTask? entity)
    {
        if (entity == null) return null;
        return new LongTaskMap
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate.ToDateTime(),
            EndDate = entity.EndDate.ToDateTime(),
        };
    }

    public static explicit operator LongTask?(LongTaskMap? longTaskMap)
    {
        if (longTaskMap == null) return null;
        return new LongTask 
        {
            Id = longTaskMap.Id,
            Name = longTaskMap.Name,
            Description = longTaskMap.Description ?? string.Empty,
            StartDate = DateOnly.FromDateTime(longTaskMap.StartDate),
            EndDate = DateOnly.FromDateTime(longTaskMap.EndDate)
        };
    }
}
