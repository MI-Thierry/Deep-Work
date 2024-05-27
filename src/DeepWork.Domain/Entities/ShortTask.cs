using Ardalis.GuardClauses;
using DeepWork.Domain.Enums;
using DeepWork.SharedKernel;

namespace DeepWork.Domain.Entities;

public class ShortTask(string name, int longTaskId, string? description = null) : EntityBase, IAggregateRoot
{
	public const int NameLength = 64;
    public const int DescriptionLength = 256;

	public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);

	public string Description { get; private set; } = Guard.Against.StringTooLong(description ?? string.Empty, DescriptionLength);

	public DateTime StartTime { get; private set; } = DateTime.MinValue;

	public DateTime EndTime { get; private set; } = DateTime.MinValue;

	public int ParentLongTaskId { get; private set; } = Guard.Against.InvalidInput(longTaskId, nameof(longTaskId), id => id > 0);

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
