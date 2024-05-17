
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.Delete;
public record DeleteLongTaskCommand(int LongTaskId) : ICommand<bool>;