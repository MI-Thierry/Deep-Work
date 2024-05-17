using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.Update;
public record UpdateLongTaskNameCommand(int Id, string Name) : ICommand<LongTaskDTO?>;
public record UpdateLongTaskDescriptionCommand(int Id, string Description) : ICommand<LongTaskDTO?>;
public record UpdateLongTaskDatesCommand(int Id, DateOnly StartDate, DateOnly EndDate) : ICommand<LongTaskDTO?>;