using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.Delete;
public class DeleteLongTaskHandle(IRepository<LongTask> repository)
    : ICommandHandler<DeleteLongTaskCommand, bool>
{
    private readonly IRepository<LongTask> _repository = repository;

    public async Task<bool> Handle(DeleteLongTaskCommand request, CancellationToken cancellationToken)
    {
        LongTask? entity = await _repository.GetByIdAsync(request.LongTaskId, cancellationToken);
        if (entity == null) return false;

        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}
