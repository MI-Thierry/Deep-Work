using DeepWork.Domain.Entities;
using SQLite;

namespace DeepWork.Infrastructure.Models;

[Table("short-tasks")]
public class ShortTaskMap
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

    public static explicit operator ShortTaskMap?(ShortTask? entity)
    {
        if (entity == null) return null;
        return new ShortTaskMap
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            LongTaskId = entity.ParentLongTaskId,
        };
    }

    public static explicit operator ShortTask?(ShortTaskMap? shortTaskMap)
    {
        if (shortTaskMap == null) return null;
        return new ShortTask()
        {
            Id = shortTaskMap.Id,
            Name = shortTaskMap.Name,
            Description = shortTaskMap.Description ?? string.Empty,
            StartTime = shortTaskMap.StartTime,
            EndTime = shortTaskMap.EndTime,
            ParentLongTaskId = shortTaskMap.LongTaskId,
        };
    }
}
