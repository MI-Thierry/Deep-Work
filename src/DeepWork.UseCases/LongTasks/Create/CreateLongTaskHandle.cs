using DeepWork.Domain.Entities;
using DeepWork.SharedKernel;
using MediatR;

namespace DeepWork.UseCases.LongTasks.Create;
public class CreateLongTaskHandle : IRequestHandler<CreateLongTaskCommand>
{
    public CreateLongTaskHandle(IRepository<LongTask> _repository)
    {
        
    }
    public Task Handle(CreateLongTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
