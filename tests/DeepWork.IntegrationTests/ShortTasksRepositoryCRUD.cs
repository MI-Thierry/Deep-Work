﻿using DeepWork.Domain.Entities;
using DeepWork.Domain.Enums;
using DeepWork.Infrastructure.Data;

namespace DeepWork.IntegrationTests;
public class ShortTasksRepositoryCRUD : BaseDeepWorkReposTest
{
	private readonly ShortTasksRepository _repository;
	public ShortTasksRepositoryCRUD()
	{
		_repository = new ShortTasksRepository(DbPath);
	}
	[Fact]
	public async void AddsShortTaskAndSetsId()
	{
		string name = Guid.NewGuid().ToString();
		string description = "Test description";
		int longTaskId = 1;

		ShortTask shortTask = new(name, longTaskId, description);
		shortTask.UpdateTimes(ShortTaskTimeType.StartTime | ShortTaskTimeType.EndTime);

		await _repository.AddAsync(shortTask);

		var newShortTask = (await _repository.ListAsync())
			.FirstOrDefault(task => task.Name == name);

		Assert.NotNull(newShortTask);
		Assert.Equal(name, newShortTask.Name);
		Assert.Equal(description, newShortTask.Description);
		Assert.Equal(longTaskId, newShortTask.ParentLongTaskId);
		Assert.True(newShortTask.Id > 0);
	}

	[Fact]
	public async void UpdatesShortTask()
	{
		string name = Guid.NewGuid().ToString();
		string description = "Test description";
		int longTaskId = 1;

		ShortTask shortTask = new(name, longTaskId, description);
		shortTask.UpdateTimes(ShortTaskTimeType.StartTime | ShortTaskTimeType.EndTime);

		await _repository.AddAsync(shortTask);
		var newTask = (await _repository.ListAsync())
			.FirstOrDefault(task => task.Name == name);

		Assert.NotNull(newTask);
		Assert.NotSame(name, newTask);

		var newName = Guid.NewGuid().ToString();
		await _repository.UpdateAsync(newTask);
		var updatedTask = await _repository.GetByIdAsync(newTask.Id);

		Assert.NotNull(updatedTask);
		Assert.NotEqual(newName, shortTask.Name);
		Assert.Equal(description, updatedTask.Description);
	}

	[Fact]
	public async void DeletesShortTask()
	{
		string name = Guid.NewGuid().ToString();
		string description = "Test description";
		int longTaskId = 1;

		ShortTask shortTask = new(name, longTaskId, description);
		shortTask.UpdateTimes(ShortTaskTimeType.StartTime | ShortTaskTimeType.EndTime);


		await _repository.AddAsync(shortTask);
		await _repository.DeleteAsync(shortTask);

		Assert.DoesNotContain(await _repository.ListAsync(), task => task.Name == name);
	}
}
