using DeepWork.Domain.Entities;

namespace DeepWork.IntegrationTests;
public class DeepWorkRepoUpdate : BaseDeepWorkRepoTest
{
    [Fact]
    public async void UpdatesLongTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = Guid.NewGuid().ToString();
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now + TimeSpan.FromDays(1);

        LongTask longTask = new()
        {
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
        };

        var repo = GetDeepWorkRepo();
        await repo.AddLongTaskAsync(longTask);
        var newTask = (await repo.GetAllLongTasksAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.NotNull(newTask);
        Assert.NotSame(newTask, longTask);

        var newName = Guid.NewGuid().ToString();
        newTask.Name = newName;
        await repo.UpdateLongTaskAsync(newTask);

        var updatedTask = await repo.GetLongTaskByIdAsync(newTask.Id);

        Assert.NotNull(updatedTask);
        Assert.NotEqual(newName, longTask.Name);
        Assert.Equal(description, updatedTask.Description);
        Assert.Equal(startDate, updatedTask.StartDate);
        Assert.Equal(endDate, updatedTask.EndDate);
    }

    [Fact]
    public async void UpdatesShortTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = Guid.NewGuid().ToString();
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now + TimeSpan.FromHours(1);
        int longTaskId = 1;

        ShortTask shortTask = new()
        {
            Name = name,
            Description = description,
            StartTime = startTime,
            EndTime = endTime,
            LongTaskId = longTaskId,
        };

        var repo = GetDeepWorkRepo();
        await repo.AddShortTaskAsync(shortTask);
        var newTask = (await repo.GetAllShortTasksAsync())
            .FirstOrDefault(task => task.Name == name);

        Assert.NotNull(newTask);
        Assert.NotSame(name, newTask);

        var newName = Guid.NewGuid().ToString();
        await repo.UpdateShortTaskAsync(newTask);
        var updatedTask = await repo.GetShortTaskByIdAsync(newTask.Id);

        Assert.NotNull(updatedTask);
        Assert.NotEqual(newName, shortTask.Name);
        Assert.Equal(description, updatedTask.Description);
        Assert.Equal(startTime, updatedTask.StartTime);
        Assert.Equal(endTime, updatedTask.EndTime);
    }
}
