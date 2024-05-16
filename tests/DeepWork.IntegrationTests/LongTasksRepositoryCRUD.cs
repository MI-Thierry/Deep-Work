using DeepWork.Domain.Entities;
using DeepWork.Infrastructure.Data;

namespace DeepWork.IntegrationTests;
public class LongTasksRepositoryCRUD : BaseDeepWorkReposTest
{
    private readonly LongTasksRepository _repository;
    public LongTasksRepositoryCRUD()
    {
        _repository = new LongTasksRepository(DbPath);
    }

    [Fact]
    public async void AddsLongTaskAndSetsId()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));

        LongTask longTask = new(name, startDate, endDate, description);

        await _repository.AddAsync(longTask);

        var newLongTask = (await _repository.ListAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.Equal(name, newLongTask?.Name);
        Assert.Equal(description, newLongTask?.Description);
        Assert.Equal(startDate, newLongTask?.StartDate);
        Assert.Equal(endDate, newLongTask?.EndDate);
        Assert.True(newLongTask?.Id > 0);
    }

    [Fact]
    public async void UpdatesLongTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));

        LongTask longTask = new(name, startDate, endDate, description);

        await _repository.AddAsync(longTask);
        var newTask = (await _repository.ListAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.NotNull(newTask);
        Assert.NotSame(newTask, longTask);

        var newName = Guid.NewGuid().ToString();
        newTask.UpdateName(newName);
        await _repository.UpdateAsync(newTask);

        var updatedTask = await _repository.GetByIdAsync(newTask.Id);

        Assert.NotNull(updatedTask);
        Assert.NotEqual(newName, longTask.Name);
        Assert.Equal(description, updatedTask.Description);
        Assert.Equal(startDate, updatedTask.StartDate);
        Assert.Equal(endDate, updatedTask.EndDate);
    }

    [Fact]
    public async void DeletesLongTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));

        LongTask longTask = new(name, startDate, endDate, description);

        await _repository.AddAsync(longTask);
        await _repository.DeleteAsync(longTask);

        Assert.DoesNotContain(await _repository.ListAsync(), entity => entity.Name == name);
    }
}
