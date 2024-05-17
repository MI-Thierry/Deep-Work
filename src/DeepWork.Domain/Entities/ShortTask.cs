using Ardalis.GuardClauses;
using DeepWork.SharedKernel;

namespace DeepWork.Domain.Entities;

public class ShortTask : EntityBase, IAggregateRoot
{
    private const string _timeMessage = "Start time and End time should in the same day which is not in past";
    public const int NameLength = 64;
    public const int DescriptionLength = 256;

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int ParentLongTaskId { get; set; }

    public ShortTask(string name, DateTime startTime, DateTime endTime, int longTaskId, string? description = null)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.StringTooLong(description ?? string.Empty, DescriptionLength);
        StartTime = Guard.Against.Expression(date => DateOnly.FromDateTime(date) != DateOnly.FromDateTime(endTime)
            || date + TimeSpan.FromMinutes(1) < DateTime.Now, startTime, _timeMessage);
        EndTime = Guard.Against.Expression(date => DateOnly.FromDateTime(date) != DateOnly.FromDateTime(startTime)
            || date + TimeSpan.FromMinutes(1) < startTime, endTime, _timeMessage);
        ParentLongTaskId = Guard.Against.InvalidInput(longTaskId, nameof(longTaskId), id => id > 0);
    }

    public ShortTask()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public void UpdateName(string updateName) => Name = Guard.Against.NullOrEmpty(updateName);

    public void UpdateTimes(DateTime startTime, DateTime endTime)
    {
        StartTime = Guard.Against.Expression(time => DateOnly.FromDateTime(time) != DateOnly.FromDateTime(EndTime)
        || time < DateTime.Now, startTime, _timeMessage);

        EndTime = Guard.Against.Expression(time => DateOnly.FromDateTime(time) != DateOnly.FromDateTime(StartTime)
        || time >= StartTime, endTime, _timeMessage);
    }

    public void UpdateDescription(string updateDescription) =>
        Description = Guard.Against.StringTooLong(updateDescription, DescriptionLength);
}
