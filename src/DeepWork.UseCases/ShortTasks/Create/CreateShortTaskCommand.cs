using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Create;
public record CreateShortTaskCommand(int ParentLongTaskId, string Name, string? Description) : ICommand<int>;
