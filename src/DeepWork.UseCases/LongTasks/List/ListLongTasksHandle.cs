using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.List;
public class ListLongTasksHandle(IRepository<LongTask> repository)
    : ICommandHandler<ListLongTasksQuery, IEnumerable<LongTaskDTO>>
{
    private readonly IRepository<LongTask> _repository = repository;

    public async Task<IEnumerable<LongTaskDTO>> Handle(ListLongTasksQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<LongTaskDTO> entities =
            (await _repository.ListAsync(cancellationToken))
            .Select(entity => new LongTaskDTO(entity.Id, entity.Name, entity.Name, entity.StartDate, entity.EndDate));

        return entities;
    }
}
