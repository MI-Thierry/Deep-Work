using DeepWork.Domain.Entities;
using SQLite;
using System.Runtime.CompilerServices;

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

		ShortTask shortTask = (ShortTask)RuntimeHelpers.GetUninitializedObject(typeof(ShortTask));
		Type shortTaskType = typeof(ShortTask);

		shortTaskType.GetProperty(nameof(ShortTask.Id))?.SetValue(shortTask, shortTaskMap.Id);
		shortTaskType.GetProperty(nameof(ShortTask.Name))?.SetValue(shortTask, shortTaskMap.Name);
		shortTaskType.GetProperty(nameof(ShortTask.Description))?.SetValue(shortTask, shortTaskMap.Description);
		shortTaskType.GetProperty(nameof(ShortTask.StartTime))?.SetValue(shortTask, shortTaskMap.StartTime);
		shortTaskType.GetProperty(nameof(ShortTask.EndTime))?.SetValue(shortTask, shortTaskMap.EndTime);
		shortTaskType.GetProperty(nameof(ShortTask.ParentLongTaskId))?.SetValue(shortTask, shortTaskMap.LongTaskId);

		return shortTask;
    }
}
