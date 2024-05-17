using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Create;
public record CreateShortTaskCommand(int ParentLongTaskId,
                                     string Name,
                                     DateTime StartTime,
                                     DateTime EndTime,
                                     string? Description) : ICommand<int>;
