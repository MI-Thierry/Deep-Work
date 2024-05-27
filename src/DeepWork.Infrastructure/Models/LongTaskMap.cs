using DeepWork.Domain.Entities;
using SQLite;
using System.Runtime.CompilerServices;

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

		LongTask longTask = (LongTask)RuntimeHelpers.GetUninitializedObject(typeof(LongTask));
		Type longTaskType = typeof(LongTask);

		longTaskType.GetProperty(nameof(LongTask.Id))?.SetValue(longTask, longTaskMap.Id);
		longTaskType.GetProperty(nameof(LongTask.Name))?.SetValue(longTask, longTaskMap.Name);
		longTaskType.GetProperty(nameof(LongTask.Description))?.SetValue(longTask, longTaskMap.Description);
		longTaskType.GetProperty(nameof(LongTask.StartDate))?.SetValue(longTask, DateOnly.FromDateTime(longTaskMap.StartDate));
		longTaskType.GetProperty(nameof(LongTask.EndDate))?.SetValue(longTask, DateOnly.FromDateTime(longTaskMap.EndDate));

		return longTask;
    }
}
