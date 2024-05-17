using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.List;
public class ListAllShortTaskHandle(IRepository<ShortTask> repository) :
    ICommandHandler<ListAllShortTasksCommand, IEnumerable<ShortTaskDTO>>
{
    private readonly IRepository<ShortTask> _repository = repository;

    public async Task<IEnumerable<ShortTaskDTO>> Handle(ListAllShortTasksCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<ShortTaskDTO> entities =
            (await _repository.ListAsync(cancellationToken))
            .Select(entity => new ShortTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartTime, entity.EndTime));

        return entities;
    }
}

public class ListShortTasksHandle(IRepository<ShortTask> shortTasksRepo)
    : ICommandHandler<ListShortTasksCommand, IEnumerable<ShortTaskDTO>>
{
    private readonly IRepository<ShortTask> _repository = shortTasksRepo;

    public async Task<IEnumerable<ShortTaskDTO>> Handle(ListShortTasksCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<ShortTaskDTO> entities =
            (await _repository.ListAsync(cancellationToken))
            .FindAll(entity => entity.ParentLongTaskId == request.LongTaskId)
            .Select(entity => new ShortTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartTime, entity.EndTime));

        return entities;
    }
}
