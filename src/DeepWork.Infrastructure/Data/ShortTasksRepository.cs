using Ardalis.Specification;
using DeepWork.Domain.Entities;
using DeepWork.Infrastructure.Models;
using DeepWork.SharedKernel;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class ShortTasksRepository : IRepository<ShortTask>
{
    private readonly SQLiteAsyncConnection _connection;
    public ShortTasksRepository(string connectionString)
    {
        _connection = new SQLiteAsyncConnection(connectionString);
        _connection.CreateTableAsync<ShortTaskMap>();
    }

    public async Task<ShortTask> AddAsync(ShortTask entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        ShortTaskMap shortTaskMap = (ShortTaskMap)entity!;
        await _connection.InsertAsync(shortTaskMap);

        // Restoring Id.
        entity.Id = shortTaskMap.Id;
        return entity;
    }

    public async Task<IEnumerable<ShortTask>> AddRangeAsync(IEnumerable<ShortTask> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        List<ShortTaskMap> shortTaskMaps = entities.Select(entity=>(ShortTaskMap)entity!).ToList();
        await _connection.InsertAsync(shortTaskMaps);

        // Restoring the Ids
        int i = 0;
        foreach (var entity in entities)
        {
            entity.Id = shortTaskMaps[i].Id;
            i++;
        }
        return entities;
    }

    public Task<bool> AnyAsync(ISpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }


    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return (await _connection.Table<ShortTaskMap>().ToListAsync()).Count != 0;
    }

    public IAsyncEnumerable<ShortTask> AsAsyncEnumerable(ISpecification<ShortTask> specification)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> CountAsync(ISpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.Table<ShortTaskMap>().CountAsync();
    }

    public async Task DeleteAsync(ShortTask entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTaskMap>().DeleteAsync(p => p.Id == entity.Id);
    }

    public async Task DeleteRangeAsync(IEnumerable<ShortTask> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTaskMap>()
            .DeleteAsync(p => entities.Any(entity => entity.Id == p.Id));
    }

    public Task DeleteRangeAsync(ISpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<ShortTask?> FirstOrDefaultAsync(ISpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<ShortTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<ShortTask?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        ArgumentNullException.ThrowIfNull(_connection);
        return (ShortTask?)(await _connection.Table<ShortTaskMap>().ToListAsync())
            .FirstOrDefault(entity => Equals(id, entity.Id));
    }

    public Task<ShortTask?> GetBySpecAsync(ISpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<ShortTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<List<ShortTask>> ListAsync(CancellationToken cancellationToken = default)
    {
        return (await _connection.Table<ShortTaskMap>().ToListAsync()).Select(entity => (ShortTask)entity!).ToList();
    }

    public Task<List<ShortTask>> ListAsync(ISpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<ShortTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<ShortTask?> SingleOrDefaultAsync(ISingleResultSpecification<ShortTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<ShortTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task UpdateAsync(ShortTask entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync((ShortTaskMap)entity!);
    }

    public async Task UpdateRangeAsync(IEnumerable<ShortTask> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAllAsync(entities.Select(entity => (ShortTaskMap)entity!));
    }
}
