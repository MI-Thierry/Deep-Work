using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.List;
public record ListAllShortTasksCommand : ICommand<IEnumerable<ShortTaskDTO>>;
public record ListShortTasksCommand(int LongTaskId) : ICommand<IEnumerable<ShortTaskDTO>>;
