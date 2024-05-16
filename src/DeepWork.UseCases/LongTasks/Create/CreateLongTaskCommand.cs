using MediatR;

namespace DeepWork.UseCases.LongTasks.Create;

public record CreateLongTaskCommand(string Name, DateOnly StartDate, DateOnly EndDate, string? Description) : IRequest;