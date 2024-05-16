using DeepWork.Infrastructure.Common;
using DeepWork.Infrastructure.Models;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class LongTaskRepository : IRepository<LongTaskDTO>
{
    private readonly SQLiteAsyncConnection _connection;

    public LongTaskRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        _connection.CreateTableAsync<LongTaskDTO>();
    }

    public async Task<LongTaskDTO> AddAsync(LongTaskDTO task)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(task);
        return task;
    }

    public async Task DeleteAllAsync()
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTaskDTO>().DeleteAsync();
    }

    public async Task DeleteAsync(LongTaskDTO task)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTaskDTO>().DeleteAsync(entity => entity.Id == task.Id);
    }

    public async Task<IEnumerable<LongTaskDTO>> GetAllAsync()
    {
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<LongTaskDTO> longTasks = await _connection.Table<LongTaskDTO>().ToListAsync();
        return longTasks;

    }

    public async Task<LongTaskDTO> GetByIdAsync(int id)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        IEnumerable<LongTaskDTO> longTasks = await _connection.Table<LongTaskDTO>().ToListAsync();
        return longTasks.Single(task => task.Id == id);

    }

    public async Task UpdateAsync(LongTaskDTO task)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync(task);
    }
}
