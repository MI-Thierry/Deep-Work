using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Delete;
public record DeleteShortTaskCommand(int ShortTaskId) : ICommand<bool>;
