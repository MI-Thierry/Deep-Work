using Ardalis.GuardClauses;
using DeepWork.SharedKernel;

namespace DeepWork.Domain.Entities;

public class LongTask : EntityBase, IAggregateRoot
{
    public const int NameLength = 64;
    public const int DescriptionLength = 1024;
    public string Name { get; set; }

    public string Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public LongTask(string name, DateOnly startDate, DateOnly endDate, string? description = null)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.StringTooLong(description ?? string.Empty, DescriptionLength);
        StartDate = Guard.Against.Expression(date => date < DateOnly.FromDateTime(DateTime.Now),
            startDate, "Start date needs to be greater than or equal to today");
        EndDate = Guard.Against.Expression(date => date < startDate,
            endDate, "End date needs to be greater than or equal to start date");
    }

    // Don't use this constructor
    public LongTask()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public void UpdateName(string name) => Name = Guard.Against.NullOrEmpty(name);

    public void UpdateStartDate(DateOnly startDate)
    {
        StartDate = Guard.Against.Expression(date => date < DateOnly.FromDateTime(DateTime.Now),
            startDate, "Start date needs to be greater than or equal to today");
    }

    public void UpdateEndDate(DateOnly endDate)
    {
        EndDate = Guard.Against.Expression(date => date < StartDate,
            endDate, "End date needs to be greater than or equal to start date");
    }

    public void UpdateDescription(string description) =>
        Description = Guard.Against.StringTooLong(description, DescriptionLength);
}
