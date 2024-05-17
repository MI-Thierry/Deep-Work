using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;

namespace DeepWork.UseCases.LongTasks.Update;
public class UpdateLongTaskNameHandle(IRepository<LongTask> repository)
    : ICommandHandler<UpdateLongTaskNameCommand, LongTaskDTO?>
{
    private readonly IRepository<LongTask> _repository = repository;

    public async Task<LongTaskDTO?> Handle(UpdateLongTaskNameCommand request, CancellationToken cancellationToken)
    {
        LongTask? entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return null;

        entity.UpdateName(request.Name);
        await _repository.UpdateAsync(entity, cancellationToken);

        return new LongTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartDate, entity.EndDate);
    }
}

public class UpdateLongTaskDescriptionHandle(IRepository<LongTask> repository)
    : ICommandHandler<UpdateLongTaskDescriptionCommand, LongTaskDTO?>
{
    private readonly IRepository<LongTask> _repository = repository;

    public async Task<LongTaskDTO?> Handle(UpdateLongTaskDescriptionCommand request, CancellationToken cancellationToken)
    {
        LongTask? entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return null;

        entity.UpdateName(request.Description);
        await _repository.UpdateAsync(entity, cancellationToken);

        return new LongTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartDate, entity.EndDate);
    }
}

public class UpdateLongTaskDatesHandle(IRepository<LongTask> repository)
    : ICommandHandler<UpdateLongTaskDatesCommand, LongTaskDTO?>
{
    private readonly IRepository<LongTask> _repository = repository;

    public async Task<LongTaskDTO?> Handle(UpdateLongTaskDatesCommand request, CancellationToken cancellationToken)
    {
        LongTask? entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return null;

        entity.UpdateDates(request.StartDate, request.EndDate);
        await _repository.UpdateAsync(entity, cancellationToken);

        return new LongTaskDTO(entity.Id, entity.Name, entity.Description, entity.StartDate, entity.EndDate);
    }
}
