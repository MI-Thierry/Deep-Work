using Ardalis.GuardClauses;
using DeepWork.SharedKernel;

namespace DeepWork.Domain.Entities;

public class LongTask(string name, DateOnly startDate, DateOnly endDate, string? description = null) : EntityBase
{
    public const int NameLength = 64;
    public const int DescriptionLength = 1024;
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name);

    public string Description { get; private set; } =
        Guard.Against.StringTooLong(description ?? string.Empty, DescriptionLength);

    public DateOnly StartDate { get; private set; } =
        Guard.Against.Expression(date => date < DateOnly.FromDateTime(DateTime.Now),
            startDate, "Start date needs to be greater than or equal to today");

    public DateOnly EndDate { get; private set; } =
        Guard.Against.Expression(date => date < startDate,
            endDate, "End date needs to be greater than or equal to start date");

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
