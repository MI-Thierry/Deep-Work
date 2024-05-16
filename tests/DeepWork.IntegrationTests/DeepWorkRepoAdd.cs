using DeepWork.Infrastructure.Models;

namespace DeepWork.IntegrationTests;

public class DeepWorkRepoAdd : BaseDeepWorkRepoTest
{
    [Fact]
    public async void AddsLongTaskAndSetsId()
    {
        string name = "Test long task";
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

        var newLongTask = (await repo.LongTaskRepository.GetAllAsync())
            .FirstOrDefault(task => task.StartDate == startDate);

        Assert.Equal(name, newLongTask?.Name);
        Assert.Equal(description, newLongTask?.Description);
        Assert.Equal(startDate, newLongTask?.StartDate);
        Assert.Equal(endDate, newLongTask?.EndDate);
        Assert.True(newLongTask?.Id > 0);
    }

    [Fact]
    public async void AddsShortTaskAndSetsId()
    {
        string name = "Test short task";
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

        var newShortTask = (await repo.ShortTaskRepository.GetAllAsync())
            .FirstOrDefault(task => task.StartTime == startTime);

        Assert.NotNull(newShortTask);
        Assert.Equal(name, newShortTask.Name);
        Assert.Equal(description, newShortTask.Description);
        Assert.Equal(startTime, newShortTask.StartTime);
        Assert.Equal(endTime, newShortTask.EndTime);
        Assert.Equal(longTaskId, newShortTask.LongTaskId);
        Assert.True(newShortTask.Id > 0);
    }
}