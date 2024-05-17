using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Delete;
public class DeleteShortTaskHandle(IRepository<ShortTask> repository)
    : ICommandHandler<DeleteShortTaskCommand, bool>
{
    private readonly IRepository<ShortTask> _repository = repository;

    public async Task<bool> Handle(DeleteShortTaskCommand request, CancellationToken cancellationToken)
    {
        ShortTask? entity = await _repository.GetByIdAsync(request.ShortTaskId, cancellationToken);
        if (entity == null) return false;

        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}
