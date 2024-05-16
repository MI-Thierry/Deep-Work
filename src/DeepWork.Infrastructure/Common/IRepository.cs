namespace DeepWork.Infrastructure.Common;
public interface IRepository<T>
{
    public Task<T> AddAsync(T task);

    public Task UpdateAsync(T task);

    public Task<T> GetByIdAsync(int id);

    public Task<IEnumerable<T>> GetAllAsync();

    public Task DeleteAsync(T task);

    public Task DeleteAllAsync();
}
