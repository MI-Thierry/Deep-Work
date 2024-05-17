using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.ShortTasks.Update;
public class UpdateShortTaskNameHandle(IRepository<ShortTask> repository)
    : ICommandHandler<UpdateShortTaskNameCommand, ShortTaskDTO?>
{
    private readonly IRepository<ShortTask> _repository = repository;

    public async Task<ShortTaskDTO?> Handle(UpdateShortTaskNameCommand request, CancellationToken cancellationToken)
    {
        ShortTask? entity = await _repository.GetByIdAsync(request.ShortTaskId, cancellationToken);
        if (entity == null) return null;

        entity.UpdateName(request.Name);
        await _repository.UpdateAsync(entity, cancellationToken);

        return new ShortTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartTime, entity.EndTime);
    }
}

public class UpdateShortTaskDescriptionHandle(IRepository<ShortTask> repository)
    : ICommandHandler<UpdateShortTaskDescriptionCommand, ShortTaskDTO?>
{
    private readonly IRepository<ShortTask> _repository = repository;

    public async Task<ShortTaskDTO?> Handle(UpdateShortTaskDescriptionCommand request, CancellationToken cancellationToken)
    {
        ShortTask? entity = await _repository.GetByIdAsync(request.ShortTaskId, cancellationToken);
        if (entity == null) return null;

        entity.UpdateDescription(request.Description);
        await _repository.UpdateAsync(entity, cancellationToken);

        return new ShortTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartTime, entity.EndTime);
    }
}

public class UpdateShortTaskTimesHandle(IRepository<ShortTask> repository)
    : ICommandHandler<UpdateShortTaskTimesCommand, ShortTaskDTO?>
{
    private readonly IRepository<ShortTask> _repository = repository;

    public async Task<ShortTaskDTO?> Handle(UpdateShortTaskTimesCommand request, CancellationToken cancellationToken)
    {
        ShortTask? entity = await _repository.GetByIdAsync(request.ShortTaskId, cancellationToken);
        if (entity == null) return null;

        entity.UpdateTimes(request.StartTime, request.EndTime);
        await _repository.UpdateAsync(entity, cancellationToken);

        return new ShortTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartTime, entity.EndTime);
    }
}
