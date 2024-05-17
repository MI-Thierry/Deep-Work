using DeepWork.Domain.Entities;
using DeepWork.Domain.Event;
using DeepWork.SharedKernel;
using MediatR;

namespace DeepWork.UseCases.LongTasks.Create;
public class CreateLongTaskHandle(IRepository<LongTask> repository, IMediator mediator)
    : ICommandHandler<CreateLongTaskCommand, int>
{
    private readonly IRepository<LongTask> _repository = repository;
    private readonly IMediator _mediator = mediator;
    public async Task<int> Handle(CreateLongTaskCommand request, CancellationToken cancellationToken)
    {
        LongTask longTask = new(request.Name, request.StartDate, request.EndDate, request.Description);
        await _repository.AddAsync(longTask, cancellationToken);

        // Notifying that LongTask is created
        LongTaskCreatedEvent @event = new(longTask.Id);
        await _mediator.Publish(@event, cancellationToken);

        return longTask.Id;
    }
}
