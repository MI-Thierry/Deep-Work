using DeepWork.Infrastructure.Data;

namespace DeepWork.Infrastructure.Common;
public  interface IDeepWorkRepositories
{
    LongTaskRepository LongTaskRepository { get; }
    ShortTaskRepository ShortTaskRepository { get; }
}
