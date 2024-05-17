using DeepWork.SharedKernel;

namespace DeepWork.Domain.Event;
public class ShortTaskCreatedEvent(int shortTaskId, int parentLongTaskId) : DomainEventBase
{
    public int ShortTaskId { get; set; } = shortTaskId;
    public int ParentLongTaskId { get; set; } = parentLongTaskId;
}
