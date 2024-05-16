using DeepWork.Infrastructure.Interfaces;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class DeepWorkRepositories : IDeepWorkRepositories
{
    private readonly string _connectionString;
    private readonly SQLiteAsyncConnection? _connection;
    public LongTasksRepository LongTaskRepository { get; private set; }
    public ShortTasksRepository ShortTaskRepository { get; private set; }

    public DeepWorkRepositories(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new SQLiteAsyncConnection(_connectionString);
        LongTaskRepository = new LongTasksRepository(_connection);
        ShortTaskRepository = new ShortTasksRepository(_connection);
    }

}
