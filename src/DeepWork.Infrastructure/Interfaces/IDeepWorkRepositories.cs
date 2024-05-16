using DeepWork.Infrastructure.Data;

namespace DeepWork.Infrastructure.Interfaces;
public  interface IDeepWorkRepositories
{
    LongTasksRepository LongTaskRepository { get; }
    ShortTasksRepository ShortTaskRepository { get; }
}
