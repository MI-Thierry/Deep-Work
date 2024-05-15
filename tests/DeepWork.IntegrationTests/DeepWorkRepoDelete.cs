using DeepWork.Domain.Entities;

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

        LongTask longTask = new()
        {
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
        };

        var repo = GetDeepWorkRepo();
        await repo.AddLongTaskAsync(longTask);
        await repo.DeleteLongTaskByIdAsync(longTask);

        Assert.DoesNotContain(await repo.GetAllLongTasksAsync(), task => task.Name == name);
    }

    [Fact]
    public async void DeletesShortTask()
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
        await repo.DeleteShortTaskByIdAsync(shortTask);

        Assert.DoesNotContain(await repo.GetAllShortTasksAsync(), task => task.Name == name);
    }
}
