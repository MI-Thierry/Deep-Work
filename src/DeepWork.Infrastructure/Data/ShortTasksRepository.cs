using Ardalis.Specification;
using DeepWork.Infrastructure.Models;
using DeepWork.SharedKernel;
using SQLite;

namespace DeepWork.Infrastructure.Data;
public class ShortTasksRepository : IRepository<ShortTaskDTO>
{
    private readonly SQLiteAsyncConnection _connection;
    public ShortTasksRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        _connection.CreateTableAsync<ShortTaskDTO>();
    }

    public async Task<ShortTaskDTO> AddAsync(ShortTaskDTO entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<ShortTaskDTO>> AddRangeAsync(IEnumerable<ShortTaskDTO> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.InsertAsync(entities);
        return entities;
    }

    public Task<bool> AnyAsync(ISpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return (await _connection.Table<ShortTaskDTO>().ToListAsync()).Count != 0;
    }

    public IAsyncEnumerable<ShortTaskDTO> AsAsyncEnumerable(ISpecification<ShortTaskDTO> specification)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> CountAsync(ISpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.Table<ShortTaskDTO>().CountAsync();
    }

    public async Task DeleteAsync(ShortTaskDTO entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTaskDTO>().DeleteAsync(p => p.Id == entity.Id);
    }

    public async Task DeleteRangeAsync(IEnumerable<ShortTaskDTO> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.Table<ShortTaskDTO>()
            .DeleteAsync(p => entities.Any(entity => entity.Id == p.Id));
    }

    public Task DeleteRangeAsync(ISpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<ShortTaskDTO?> FirstOrDefaultAsync(ISpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<ShortTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<ShortTaskDTO?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        ArgumentNullException.ThrowIfNull(_connection);
        return (await _connection.Table<ShortTaskDTO>().ToListAsync())
            .FirstOrDefault(entity => Equals(id, entity.Id));
    }

    public Task<ShortTaskDTO?> GetBySpecAsync(ISpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<ShortTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task<List<ShortTaskDTO>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.Table<ShortTaskDTO>().ToListAsync();
    }

    public Task<List<ShortTaskDTO>> ListAsync(ISpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<ShortTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<ShortTaskDTO?> SingleOrDefaultAsync(ISingleResultSpecification<ShortTaskDTO> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<ShortTaskDTO, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Todo: Support this
        throw new NotSupportedException();
    }

    public async Task UpdateAsync(ShortTaskDTO entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAsync(entity);
    }

    public async Task UpdateRangeAsync(IEnumerable<ShortTaskDTO> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_connection);
        await _connection.UpdateAllAsync(entities);
    }
}
