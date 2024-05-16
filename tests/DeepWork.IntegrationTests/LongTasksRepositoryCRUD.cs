using DeepWork.Infrastructure.Models;

namespace DeepWork.IntegrationTests;
public class LongTasksRepositoryCRUD : BaseDeepWorkRepoTest
{
    [Fact]
    public async void AddsLongTaskAndSetsId()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now + TimeSpan.FromDays(1);

        LongTaskDTO longTask = new()
        {
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
        };

        var repo = GetDeepWorkRepo();
        await repo.LongTaskRepository.AddAsync(longTask);

        var newLongTask = (await repo.LongTaskRepository.ListAsync())
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
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now + TimeSpan.FromDays(1);

        LongTaskDTO longTask = new()
        {
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
        };

        var repo = GetDeepWorkRepo();
        await repo.LongTaskRepository.AddAsync(longTask);
        var newTask = (await repo.LongTaskRepository.ListAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.NotNull(newTask);
        Assert.NotSame(newTask, longTask);

        var newName = Guid.NewGuid().ToString();
        newTask.Name = newName;
        await repo.LongTaskRepository.UpdateAsync(newTask);

        var updatedTask = await repo.LongTaskRepository.GetByIdAsync(newTask.Id);

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
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now + TimeSpan.FromDays(1);

        LongTaskDTO longTask = new()
        {
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
        };

        var repo = GetDeepWorkRepo();
        await repo.LongTaskRepository.AddAsync(longTask);
        await repo.LongTaskRepository.DeleteAsync(longTask);

        Assert.DoesNotContain(await repo.LongTaskRepository.ListAsync(), entity => entity.Name == name);
    }
}
