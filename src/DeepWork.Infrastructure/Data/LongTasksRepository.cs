using Ardalis.Specification;
using DeepWork.Domain.Entities;
using DeepWork.Infrastructure.Models;
using DeepWork.SharedKernel;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class LongTasksRepository : IRepository<LongTask>
{
    private readonly SQLiteAsyncConnection _connection;

    public LongTasksRepository(string connectionString)
    {
        _connection = new SQLiteAsyncConnection(connectionString);
        _connection.CreateTableAsync<LongTaskDTO>().Wait();
    }

    public async Task<LongTask> AddAsync(LongTask entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        LongTaskDTO longTaskDTO = (LongTaskDTO)entity!;
        await _connection.InsertAsync(longTaskDTO);

        // Restoring the Id
        entity.Id = longTaskDTO.Id;
        return entity;
    }

    public async Task<IEnumerable<LongTask>> AddRangeAsync(IEnumerable<LongTask> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        List<LongTaskDTO> longTaskDTOs = entities.Select(entity => (LongTaskDTO)entity!).ToList();
        await _connection.InsertAllAsync(longTaskDTOs);

        // Restoring the Ids
        int i = 0;
        foreach(var entity in entities)
        {
            entity.Id = longTaskDTOs[i].Id;
            i++;
        }
        return entities;
    }

    public Task<bool> AnyAsync(ISpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return (await _connection.Table<LongTaskDTO>().ToListAsync()).Count != 0;
    }

    public IAsyncEnumerable<LongTask> AsAsyncEnumerable(ISpecification<LongTask> specification)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> CountAsync(ISpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.Table<LongTaskDTO>().CountAsync();
    }

    public async Task DeleteAsync(LongTask entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTaskDTO>().DeleteAsync(p => p.Id == entity.Id);
    }

    public async Task DeleteRangeAsync(IEnumerable<LongTask> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTaskDTO>()
            .DeleteAsync(p => entities.Any(entity => entity.Id == p.Id));
    }

    public Task DeleteRangeAsync(ISpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<LongTask?> FirstOrDefaultAsync(ISpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<LongTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<LongTask?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        ArgumentNullException.ThrowIfNull(_connection);
       return (LongTask?)(await _connection.Table<LongTaskDTO>()
            .ToListAsync()).FirstOrDefault(p => Equals(id, p.Id));
    }

    public Task<LongTask?> GetBySpecAsync(ISpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<LongTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<List<LongTask>> ListAsync(CancellationToken cancellationToken = default)
    {
        return (await _connection.Table<LongTaskDTO>().ToListAsync()).Select(entity => (LongTask?)entity).ToList()!;
    }

    public Task<List<LongTask>> ListAsync(ISpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<LongTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<LongTask?> SingleOrDefaultAsync(ISingleResultSpecification<LongTask> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<LongTask, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task UpdateAsync(LongTask entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync((LongTaskDTO)entity!);
    }

    public async Task UpdateRangeAsync(IEnumerable<LongTask> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAllAsync(entities.Select(entity => (LongTaskDTO)entity!));
    }
}
