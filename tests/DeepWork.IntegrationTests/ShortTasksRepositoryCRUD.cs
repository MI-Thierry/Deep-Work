using DeepWork.Infrastructure.Models;

namespace DeepWork.IntegrationTests;
public class ShortTasksRepositoryCRUD : BaseDeepWorkRepoTest
{
    [Fact]
    public async void AddsShortTaskAndSetsId()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now + TimeSpan.FromHours(1);
        int longTaskId = 1;

        ShortTaskDTO shortTask = new()
        {
            Name = name,
            Description = description,
            StartTime = startTime,
            EndTime = endTime,
            LongTaskId = longTaskId
        };

        var repo = GetDeepWorkRepo();
        await repo.ShortTaskRepository.AddAsync(shortTask);

        var newShortTask = (await repo.ShortTaskRepository.ListAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.NotNull(newShortTask);
        Assert.Equal(name, newShortTask.Name);
        Assert.Equal(description, newShortTask.Description);
        Assert.Equal(startTime, newShortTask.StartTime);
        Assert.Equal(endTime, newShortTask.EndTime);
        Assert.Equal(longTaskId, newShortTask.LongTaskId);
        Assert.True(newShortTask.Id > 0);
    }

    [Fact]
    public async void UpdatesShortTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now + TimeSpan.FromHours(1);
        int longTaskId = 1;

        ShortTaskDTO shortTask = new()
        {
            Name = name,
            Description = description,
            StartTime = startTime,
            EndTime = endTime,
            LongTaskId = longTaskId,
        };

        var repo = GetDeepWorkRepo();
        await repo.ShortTaskRepository.AddAsync(shortTask);
        var newTask = (await repo.ShortTaskRepository.ListAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.NotNull(newTask);
        Assert.NotSame(name, newTask);

        var newName = Guid.NewGuid().ToString();
        await repo.ShortTaskRepository.UpdateAsync(newTask);
        var updatedTask = await repo.ShortTaskRepository.GetByIdAsync(newTask.Id);

        Assert.NotNull(updatedTask);
        Assert.NotEqual(newName, shortTask.Name);
        Assert.Equal(description, updatedTask.Description);
        Assert.Equal(startTime, updatedTask.StartTime);
        Assert.Equal(endTime, updatedTask.EndTime);
    }

    [Fact]
    public async void DeletesShortTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = "Test description";
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now + TimeSpan.FromHours(1);
        int longTaskId = 1;

        ShortTaskDTO shortTask = new()
        {
            Name = name,
            Description = description,
            StartTime = startTime,
            EndTime = endTime,
            LongTaskId = longTaskId,
        };

        var repo = GetDeepWorkRepo();
        await repo.ShortTaskRepository.AddAsync(shortTask);
        await repo.ShortTaskRepository.DeleteAsync(shortTask);

        Assert.DoesNotContain(await repo.ShortTaskRepository.ListAsync(), task => task.Name == name);
    }
}
