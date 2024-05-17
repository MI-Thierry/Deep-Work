using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.List;
public record ListLongTasksQuery : ICommand<IEnumerable<LongTaskDTO>>;