using DeepWork.Domain.Enums;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Update;

public record UpdateShortTaskNameCommand(int ShortTaskId, string Name) : ICommand<ShortTaskDTO?>;
public record UpdateShortTaskDescriptionCommand(int ShortTaskId, string Description) : ICommand<ShortTaskDTO?>;
public record UpdateShortTaskTimesCommand(int ShortTaskId, ShortTaskTimeType TimeType) : ICommand<ShortTaskDTO?>;