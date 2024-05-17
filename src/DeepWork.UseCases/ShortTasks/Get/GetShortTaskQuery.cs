using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Get;
public record GetShortTaskQuery(int ShortTaskId) : IQuery<ShortTaskDTO?>;
