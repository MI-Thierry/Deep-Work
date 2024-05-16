using DeepWork.Infrastructure.Models;

namespace DeepWork.IntegrationTests;
public class DeepWorkRepoDelete : BaseDeepWorkRepoTest
{
    [Fact]
    public async void DeletesLongTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = Guid.NewGuid().ToString();
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

        Assert.DoesNotContain(await repo.LongTaskRepository.GetAllAsync(), task => task.Name == name);
    }

    [Fact]
    public async void DeletesShortTask()
    {
        string name = Guid.NewGuid().ToString();
        string description = Guid.NewGuid().ToString();
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

        Assert.DoesNotContain(await repo.ShortTaskRepository.GetAllAsync(), task => task.Name == name);
    }
}
