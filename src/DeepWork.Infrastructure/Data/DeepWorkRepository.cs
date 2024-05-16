using DeepWork.Infrastructure.Common;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class DeepWorkRepository : IDeepWorkRepository
{
    private readonly string _connectionString;
    private readonly SQLiteAsyncConnection? _connection;
    public LongTaskRepository LongTaskRepository { get; private set; }
    public ShortTaskRepository ShortTaskRepository { get; private set; }

    public DeepWorkRepository(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new SQLiteAsyncConnection(_connectionString);
        LongTaskRepository = new LongTaskRepository(_connection);
        ShortTaskRepository = new ShortTaskRepository(_connection);
    }

}
