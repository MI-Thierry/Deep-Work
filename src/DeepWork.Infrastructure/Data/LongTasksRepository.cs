using Ardalis.Specification;
using DeepWork.Infrastructure.Models;
using DeepWork.SharedKernel;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class LongTasksRepository : IRepository<LongTaskDTO>
{
    private readonly SQLiteAsyncConnection _connection;

    public LongTasksRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        _connection.CreateTableAsync<LongTaskDTO>();
    }

    public async Task<LongTaskDTO> AddAsync(LongTaskDTO entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<LongTaskDTO>> AddRangeAsync(IEnumerable<LongTaskDTO> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAllAsync(entities);
        return entities;
    }

    public Task<bool> AnyAsync(ISpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return (await _connection.Table<LongTaskDTO>().ToListAsync()).Count != 0;
    }

    public IAsyncEnumerable<LongTaskDTO> AsAsyncEnumerable(ISpecification<LongTaskDTO> specification)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> CountAsync(ISpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.Table<LongTaskDTO>().CountAsync();
    }

    public async Task DeleteAsync(LongTaskDTO entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTaskDTO>().DeleteAsync(p => p.Id == entity.Id);
    }

    public async Task DeleteRangeAsync(IEnumerable<LongTaskDTO> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<LongTaskDTO>()
            .DeleteAsync(p => entities.Any(entity => entity.Id == p.Id));
    }

    public Task DeleteRangeAsync(ISpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<LongTaskDTO?> FirstOrDefaultAsync(ISpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<LongTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<LongTaskDTO?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        ArgumentNullException.ThrowIfNull(_connection);
        return (await _connection.Table<LongTaskDTO>()
            .ToListAsync()).FirstOrDefault(entity => Equals(id, entity.Id));
    }

    public Task<LongTaskDTO?> GetBySpecAsync(ISpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<LongTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<List<LongTaskDTO>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.Table<LongTaskDTO>().ToListAsync();
    }

    public Task<List<LongTaskDTO>> ListAsync(ISpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<LongTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<LongTaskDTO?> SingleOrDefaultAsync(ISingleResultSpecification<LongTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<LongTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task UpdateAsync(LongTaskDTO entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync(entity);
    }

    public async Task UpdateRangeAsync(IEnumerable<LongTaskDTO> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAllAsync(entities);
    }
}
