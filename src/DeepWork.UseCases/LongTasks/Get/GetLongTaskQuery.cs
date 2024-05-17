using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.Get;
public record GetLongTaskQuery(int LongTaskId) : IQuery<LongTaskDTO?>;
