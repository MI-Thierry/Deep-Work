using DeepWork.Infrastructure.Common;
using DeepWork.Infrastructure.Models;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class ShortTaskRepository : IRepository<ShortTaskDTO>
{
    private readonly SQLiteAsyncConnection _connection;
    public ShortTaskRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        _connection.CreateTableAsync<ShortTaskDTO>();
    }

    public async Task<ShortTaskDTO> AddAsync(ShortTaskDTO task)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(task);
        return task;

    }

    public async Task DeleteAllAsync()
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTaskDTO>().DeleteAsync();

    }

    public async Task DeleteAsync(ShortTaskDTO task)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTaskDTO>().DeleteAsync(entity => entity.Id == task.Id);
    }

    public async Task<IEnumerable<ShortTaskDTO>> GetAllAsync()
    {
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<ShortTaskDTO> longTasks = await _connection.Table<ShortTaskDTO>().ToListAsync();
        return longTasks;
    }

    public async Task<ShortTaskDTO> GetByIdAsync(int id)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<ShortTaskDTO> longTasks = await _connection.Table<ShortTaskDTO>().ToListAsync();
        return longTasks.Single(task => task.Id == id);
    }

    public async Task UpdateAsync(ShortTaskDTO task)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync(task);
    }
}
