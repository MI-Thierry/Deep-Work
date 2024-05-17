using DeepWork.Domain.Entities;
using DeepWork.Domain.Event;
using DeepWork.SharedKernel;
using MediatR;

namespace DeepWork.UseCases.ShortTasks.Create;
public class CreateShortTaskHandle(IRepository<ShortTask> repository, IMediator mediator)
    : ICommandHandler<CreateShortTaskCommand, int>
{
    private readonly IRepository<ShortTask> _repository = repository;
    private readonly IMediator _mediator = mediator;

    public async Task<int> Handle(CreateShortTaskCommand request, CancellationToken cancellationToken)
    {
        ShortTask entity = new(request.Name, request.StartTime, request.EndTime, request.ParentLongTaskId, request.Description);
        await _repository.AddAsync(entity, cancellationToken);

        await _mediator.Publish(new ShortTaskCreatedEvent(entity.Id, entity.ParentLongTaskId), cancellationToken);
        return entity.Id;
    }
}
