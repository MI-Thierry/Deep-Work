using DeepWork.Infrastructure.Data;

namespace DeepWork.Infrastructure.Common;
public  interface IDeepWorkRepository
{
    LongTaskRepository LongTaskRepository { get; }
    ShortTaskRepository ShortTaskRepository { get; }
}
