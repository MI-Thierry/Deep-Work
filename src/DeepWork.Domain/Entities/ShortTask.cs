using Ardalis.GuardClauses;
using DeepWork.SharedKernel;

namespace DeepWork.Domain.Entities;

public class ShortTask(string name, DateTime startTime, DateTime endTime, int longTaskId, string? description = null) : EntityBase
{
    private const string _timeMessage = "Start time and End time should in the same day which is not in past";
    public const int NameLength = 64;
    public const int DescriptionLength = 256;
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);

    public string? Description { get; private set; } =
        Guard.Against.StringTooLong(description ?? string.Empty, DescriptionLength);

    public DateTime StartTime { get; private set; } =
        Guard.Against.Expression(date => DateOnly.FromDateTime(date) != DateOnly.FromDateTime(endTime)
        || date < DateTime.Now, startTime, _timeMessage);

    public DateTime EndTime { get; private set; } =
        Guard.Against.Expression(date => DateOnly.FromDateTime(date) != DateOnly.FromDateTime(startTime)
        || date < startTime, endTime, _timeMessage);

    public int LongTaskId { get; private set; } =
        Guard.Against.InvalidInput(longTaskId, nameof(longTaskId), id => id > 0);

    public void UpdateName(string updateName) => Name = Guard.Against.NullOrEmpty(updateName);

    public void UpdateStartDate(DateTime updateTime)
    {
        StartTime = Guard.Against.Expression(time => DateOnly.FromDateTime(time) != DateOnly.FromDateTime(EndTime)
        || time < DateTime.Now, updateTime, _timeMessage);
    }

    public void UpdateEndDate(DateTime updateTime)
    {
        EndTime = Guard.Against.Expression(time => DateOnly.FromDateTime(time) != DateOnly.FromDateTime(StartTime)
        || time >= StartTime, updateTime, _timeMessage);
    }

    public void UpdateDescription(string updateDescription) =>
        Description = Guard.Against.StringTooLong(updateDescription, DescriptionLength);
}
