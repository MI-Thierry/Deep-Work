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

    public static explicit operator LongTaskDTO?(LongTask? entity)
    {
        if (entity == null) return null;
        return new LongTaskDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate.ToDateTime(),
            EndDate = entity.EndDate.ToDateTime(),
        };
    }

    public static explicit operator LongTask?(LongTaskDTO? longTaskDTO)
    {
        if (longTaskDTO == null) return null;
        LongTask longTask = new()
        {
            Id = longTaskDTO.Id,
            Name = longTaskDTO.Name,
            Description = longTaskDTO.Description ?? string.Empty,
            StartDate = DateOnly.FromDateTime(longTaskDTO.StartDate),
            EndDate = DateOnly.FromDateTime(longTaskDTO.EndDate)
        };
        return longTask;
    }
}
