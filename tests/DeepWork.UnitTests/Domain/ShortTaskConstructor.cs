﻿using DeepWork.Domain.Entities;
using DeepWork.Domain.Enums;

namespace DeepWork.UnitTests.Domain;

public class ShortTaskConstructor
{
    private readonly string _name = Guid.NewGuid().ToString();
    private readonly string _smallDesc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras ut tellus eu mauris tincidunt laoreet.\r\n";
    private readonly string _largeDesc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent leo orci, varius eget scelerisque et, pulvinar sed sapien. Phasellus eget orci non lorem blandit ornare sed a erat. In a dignissim ante, ac convallis urna. Mauris quis luctus tellus. Mauris viverra nisl eu nunc tincidunt, sed lacinia erat dictum. Vestibulum ac justo vitae eros pretium sodales. Nam egestas libero vitae feugiat placerat. Maecenas luctus congue mi vitae tristique. Cras sit amet auctor odio. Suspendisse eleifend ligula at turpis vehicula porta. Sed tincidunt sapien pellentesque diam malesuada condimentum. Pellentesque libero nisi, vehicula non est eget, laoreet fermentum quam. Etiam vehicula rutrum ligula, id laoreet sem lobortis in. Nullam gravida orci est, non posuere nunc rhoncus a. Aliquam vel tincidunt mauris, et auctor nisi.\r\n\r\nPraesent ultricies quam neque, vitae tempor nisl ultricies ut. Nam lobortis sapien sed arcu pulvinar egestas. Phasellus eu mi nunc. Proin sed libero lorem. Nullam eros nunc, eleifend nec lectus at, cursus laoreet turpis. Sed commodo pharetra ex, at semper nisl hendrerit ut. Aenean et vehicula ligula. Curabitur vitae purus justo. Nullam ex diam, ultricies at eros vitae, ultricies pulvinar neque. Suspendisse at lorem id diam lacinia dictum.\r\n\r\nAenean faucibus nisl vitae rhoncus ullamcorper. Vivamus sollicitudin mi quis scelerisque condimentum. Etiam ultrices aliquet lectus mauris.";
    private readonly int _longTaskId = 1;

    [Fact]
    public void InitializeShortTask()
    {
        ShortTask shortTask = new(_name, _longTaskId, _smallDesc);
		shortTask.UpdateTimes(ShortTaskTimeType.StartTime | ShortTaskTimeType.EndTime);

        Assert.Equal(_name, shortTask.Name);
        Assert.Equal(_smallDesc, shortTask.Description);
        Assert.Equal(_longTaskId, shortTask.ParentLongTaskId);
		Assert.NotEqual(DateTime.MinValue, shortTask.StartTime);
		Assert.NotEqual(DateTime.MinValue, shortTask.EndTime);

        Assert.ThrowsAny<ArgumentException>(() => new ShortTask(string.Empty, _longTaskId, _smallDesc));
        Assert.ThrowsAny<ArgumentException>(() => new ShortTask(_name, _longTaskId, _largeDesc));
    }
}
