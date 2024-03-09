using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using DeepWork.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeepWork.ViewModels.Pages;

public partial class TaskManagementViewModel : ObservableObject
{
	private string _selectedLongTaskName;
	private string _selectedShortTaskName;
	private readonly AccountManagementService _accountManager;

	public LongTaskViewModel SelectedLongTask { get; set; }
	public ShortTaskViewModel SelectedShortTask { get; set; }


	[ObservableProperty]
	private ObservableCollection<LongTaskViewModel> _longTasks;

	[ObservableProperty]
	private ObservableCollection<ShortTaskViewModel> _shortTasks;

	public TaskManagementViewModel(AccountManagementService accountManager)
	{
		_accountManager = accountManager;
		_longTasks = new ObservableCollection<LongTaskViewModel>();
		_shortTasks = new ObservableCollection<ShortTaskViewModel>();

		foreach (var task in _accountManager.UserAccount.LongTasks)
			_longTasks.Add(task);

		if (_longTasks.Any())
			SelectLongTask(_longTasks.First());
	}

	public void SelectLongTask(LongTaskViewModel taskVm)
	{
		SelectedLongTask = _accountManager.GetLongTaskByName(taskVm.Name);
		_selectedLongTaskName = SelectedLongTask.Name;

		ShortTasks.Clear();
		foreach (var task in _accountManager.GetLongTaskByName(_selectedLongTaskName).RunningTasks)
			ShortTasks.Add(task);
	}

	public void SelectShortTask(ShortTaskViewModel taskVm)
	{
		SelectedShortTask = _accountManager.GetShortTaskByName(_selectedLongTaskName, taskVm.Name);
		_selectedShortTaskName = SelectedShortTask.Name;
	}

	public void AddLongTask(LongTaskViewModel task)
	{
		LongTasks.Add(task);
		_accountManager.AddLongTask(task);
	}

	public void EditSelectedLongTask(LongTaskViewModel editedTask)
	{
		_accountManager.EditLongTask(_selectedLongTaskName, editedTask);
		LongTaskViewModel taskToEdit = LongTasks.First(task => task.Name == _selectedLongTaskName);
		taskToEdit.Name = editedTask.Name;
		taskToEdit.StartDate = editedTask.StartDate;
		taskToEdit.EndDate = editedTask.EndDate;
		SelectLongTask(taskToEdit);
	}

	public void FinishLongTask(string name)
	{
		LongTasks.Remove(LongTasks.FirstOrDefault(item => item.Name == name));
		_accountManager.FinishLongTask(name);
	}

	public void AddShortTask(ShortTaskViewModel task)
	{
		ShortTasks.Add(task);
		LongTasks.First(t => t.Name == _selectedLongTaskName).TaskCount++;
		_accountManager.AddShortTask(_selectedLongTaskName, task);
	}

	public void EditShortTask(ShortTaskViewModel editedTask)
	{
		_accountManager.EditShortTask(_selectedLongTaskName, _selectedShortTaskName, editedTask);
		ShortTaskViewModel taskToEdit = ShortTasks.First(task => task.Name == _selectedShortTaskName);
		taskToEdit.Name = editedTask.Name;
		taskToEdit.Duration = editedTask.Duration;
		SelectShortTask(taskToEdit);
	}

	public void FinishShortTask(string name)
	{
		ShortTasks.Remove(ShortTasks.FirstOrDefault(item => item.Name == name));
		LongTasks.First(t => t.Name == _selectedLongTaskName).TaskCount--;
		_accountManager.FinishShortTask(_selectedLongTaskName, name);
	}

	private List<LongTask> PopulateLongTasks()
	{
		return new List<LongTask>
		{
			new() {
				Name = "First Long Task",
				StartDate = DateTimeOffset.Now,
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(1),
				MaxDuration = TimeSpan.Zero,
				MaxShortTaskCount = 0,

				RunningTasks = new List<ShortTask>
				{
					new() {
						Duration = TimeSpan.Zero,
						Name = "First Task",
						FinishDate = DateTimeOffset.Now,
					},
					new() {
						Duration = TimeSpan.Zero,
						Name = "Second Task",
						FinishDate = DateTimeOffset.Now,
					}
				}
			},
			new() {
				Name = "Second Long Task",
				StartDate = DateTimeOffset.Now,
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(1),
				MaxDuration = TimeSpan.Zero,
				MaxShortTaskCount = 0,

				RunningTasks = new List<ShortTask>
				{
					new() {
						Duration = TimeSpan.Zero,
						Name = "Second Task",
						FinishDate = DateTimeOffset.Now,
					}
				}
			}
		};
	}
}
