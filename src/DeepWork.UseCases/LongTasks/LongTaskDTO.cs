namespace DeepWork.UseCases.LongTasks;

public record LongTaskDTO(int LongTaskId,
                          string Name,
                          string Description,
                          DateOnly StartDate,
                          DateOnly EndDate);

