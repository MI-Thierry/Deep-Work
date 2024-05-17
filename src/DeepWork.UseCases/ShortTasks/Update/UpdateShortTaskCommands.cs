using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Update;
public record UpdateShortTaskNameCommand(int ShortTaskId, string Name) : ICommand<ShortTaskDTO?>;
public record UpdateShortTaskDescriptionCommand(int ShortTaskId, string Description) : ICommand<ShortTaskDTO?>;
public record UpdateShortTaskTimesCommand(int ShortTaskId, DateTime StartTime, DateTime EndTime) : ICommand<ShortTaskDTO?>;