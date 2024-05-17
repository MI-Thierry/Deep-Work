using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Get;
public class GetShortTaskHandle(IRepository<ShortTask> repository)
    : IQueryHandler<GetShortTaskQuery, ShortTaskDTO?>
{
    private readonly IRepository<ShortTask> _repository = repository;

    public async Task<ShortTaskDTO?> Handle(GetShortTaskQuery request, CancellationToken cancellationToken)
    {
        ShortTask? entity = await _repository.GetByIdAsync(request.ShortTaskId, cancellationToken);
        if (entity == null) return null;

        return new ShortTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartTime, entity.EndTime);
    }
}
