using DeepWork.Domain.Entities;
using DeepWork.Infrastructure.Common;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class DeepWorkRepository(string connectionString) : IRepository, IDisposable
{
    private readonly string _connectionString = connectionString;
    private SQLiteAsyncConnection? _connection;

    public async Task Init()
    {
        if (_connection != null)
            return;

        _connection = new SQLiteAsyncConnection(_connectionString);
        await _connection.CreateTableAsync<LongTask>();
        await _connection.CreateTableAsync<ShortTask>();
    }

    public async Task<LongTask> AddLongTaskAsync(LongTask task)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(task);
        return task;
    }
    public async Task UpdateLongTaskAsync(LongTask task)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync(task);
    }

    public async Task<LongTask> GetLongTaskByIdAsync(int id)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<LongTask> longTasks = await _connection.Table<LongTask>().ToListAsync();
        return longTasks.Single(task => task.Id == id);
    }

    public async Task<IEnumerable<LongTask>> GetAllLongTasksAsync()
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<LongTask> longTasks = await _connection.Table<LongTask>().ToListAsync();
        return longTasks;
    }

    public async Task DeleteLongTaskByIdAsync(int id)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTask>().DeleteAsync(task => task.Id == id);
    }

    public async Task DeleteAllLongTasksAsync()
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTask>().DeleteAsync();
    }

    public async Task<ShortTask> AddShortTaskAsync(ShortTask task)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(task);
        return task;
    }

    public async Task UpdateShortTaskAsync(ShortTask task)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync(task);
    }

    public async Task<ShortTask> GetShortTaskByIdAsync(int id)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<ShortTask> longTasks = await _connection.Table<ShortTask>().ToListAsync();
        return longTasks.Single(task => task.Id == id);
    }

    public async Task<IEnumerable<ShortTask>> GetAllShortTasksAsync()
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<ShortTask> longTasks = await _connection.Table<ShortTask>().ToListAsync();
        return longTasks;
    }

    public async Task DeleteShortTaskByIdAsync(int id)
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTask>().DeleteAsync(task => task.Id == id);
    }

    public async Task DeleteAllShortTasksAsync()
    {
        await Init();
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTask>().DeleteAsync();
    }

    // For testing purposes
    public void Dispose()
    {
        _connection?.CloseAsync().Wait();
    }
}
