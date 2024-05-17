using DeepWork.SharedKernel;

namespace DeepWork.Domain.Event;

public class LongTaskCreatedEvent(int longTask) : DomainEventBase
{
    public int LongTaskId { get; set; } = longTask;
}
