using Ardalis.GuardClauses;
using DeepWork.Domain.Enums;
using DeepWork.SharedKernel;

namespace DeepWork.Domain.Entities;

public class ShortTask : EntityBase, IAggregateRoot
{
    private const string _timeMessage = "Start time and End time should in the same day which is not in past";
    public const int NameLength = 64;
    public const int DescriptionLength = 256;

    public string Name { get; set; }

    public string Description { get; set; }

	public DateTime StartTime { get; set; } = DateTime.MinValue;

	public DateTime EndTime { get; set; } = DateTime.MinValue;

    public int ParentLongTaskId { get; set; }

    public ShortTask(string name, int longTaskId, string? description = null)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.StringTooLong(description ?? string.Empty, DescriptionLength);
        ParentLongTaskId = Guard.Against.InvalidInput(longTaskId, nameof(longTaskId), id => id > 0);
    }

    public ShortTask()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public void UpdateName(string updateName) => Name = Guard.Against.NullOrEmpty(updateName);

    public void UpdateTimes(ShortTaskTimeType timeType)
    {
		if (timeType == ShortTaskTimeType.StartTime)
			StartTime = DateTime.Now;
		else if (timeType == ShortTaskTimeType.EndTime)
			EndTime = DateTime.Now;
		else if (timeType == (ShortTaskTimeType.StartTime | ShortTaskTimeType.EndTime))
			StartTime = EndTime = DateTime.Now;
    }

    public void UpdateDescription(string updateDescription) =>
        Description = Guard.Against.StringTooLong(updateDescription, DescriptionLength);
}
