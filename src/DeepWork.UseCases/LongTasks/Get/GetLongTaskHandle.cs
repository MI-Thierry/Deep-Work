using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.Get;
public class GetLongTaskHandle(IRepository<LongTask> repository) : IQueryHandler<GetLongTaskQuery, LongTaskDTO?>
{
    private readonly IRepository<LongTask> _repository = repository;

    public async Task<LongTaskDTO?> Handle(GetLongTaskQuery request, CancellationToken cancellationToken)
    {
        LongTask? entity = await _repository.GetByIdAsync(request.LongTaskId, cancellationToken);
        if (entity == null) return null;

        return new LongTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartDate, entity.EndDate);
    }
}
