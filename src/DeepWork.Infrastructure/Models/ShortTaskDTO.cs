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

    public static explicit operator ShortTaskDTO?(ShortTask? entity)
    {
        if (entity == null) return null;
        return new ShortTaskDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            LongTaskId = entity.LongTaskId,
        };
    }

    public static explicit operator ShortTask?(ShortTaskDTO? shortTaskDTO)
    {
        if (shortTaskDTO == null) return null;
        return new ShortTask()
        {
            Id = shortTaskDTO.Id,
            Name = shortTaskDTO.Name,
            Description = shortTaskDTO.Description ?? string.Empty,
            StartTime = shortTaskDTO.StartTime,
            EndTime = shortTaskDTO.EndTime,
            LongTaskId = shortTaskDTO.LongTaskId,
        };
    }
}
