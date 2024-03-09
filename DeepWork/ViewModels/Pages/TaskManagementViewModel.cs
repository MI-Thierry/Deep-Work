using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.Models;
using DeepWork.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeepWork.ViewModels.Pages
{
	public partial class TaskManagementViewModel : ObservableObject
	{
		private readonly AccountManagementService _accountManager;
		private LongTask _selectedLongTask;
		private List<LongTask> _tempLongTasks;


		[ObservableProperty]
		private ObservableCollection<LongTaskViewModel> _longTasks;

		[ObservableProperty]
		private ObservableCollection<ShortTaskViewModel> _shortTasks;

		public TaskManagementViewModel(AccountManagementService accountManager)
		{
			_accountManager = accountManager;
			_longTasks = new ObservableCollection<LongTaskViewModel>();
			_shortTasks = new ObservableCollection<ShortTaskViewModel>();

			_tempLongTasks = PopulateLongTasks();
			foreach (var task in _tempLongTasks)
				_longTasks.Add(task);

			if (_longTasks.Any())
				SelectLongTask(_longTasks.First());
		}

		public void SelectLongTask(LongTaskViewModel taskVm)
		{
			_selectedLongTask = _tempLongTasks.First(x => x.Name == taskVm.Name);

			ShortTasks.Clear();
			foreach (var task in _selectedLongTask.RunningTasks)
				ShortTasks.Add(task);
		}

		public void AddLongTask(LongTaskViewModel task)
		{
			LongTasks.Add(task);
			_tempLongTasks.Add(task);
			_accountManager.AddLongTask(task);
		}

		public void AddShortTask(ShortTaskViewModel task)
		{
			ShortTasks.Add(task);
			LongTasks.First(t => t.Name == _selectedLongTask.Name).TaskCount++;
			_accountManager.AddShortTask(_selectedLongTask, task);
		}

		public void FinishLongTask(string name)
		{
			LongTasks.Remove(LongTasks.FirstOrDefault(item => item.Name == name));
			_accountManager.FinishLongTask(name);
		}

		public void FinishShortTask(string name)
		{
			ShortTasks.Remove(ShortTasks.FirstOrDefault(item => item.Name == name));
			LongTasks.First(t => t.Name == _selectedLongTask.Name).TaskCount--;
			_accountManager.FinishShortTask(_selectedLongTask, name);
		}

		private List<LongTask> PopulateLongTasks()
		{
			return new List<LongTask>
			{
				new() {
					Name = "First Long Task",
					StartDate = DateTime.Now,
					EndDate = DateTime.Now + TimeSpan.FromDays(1),
					MaxDuration = TimeSpan.Zero,
					MaxShortTaskCount = 0,

					RunningTasks = new List<ShortTask>
					{
						new() {
							Duration = TimeSpan.Zero,
							Name = "First Task",
							FinishDate = DateTime.Now,
						},
						new() {
							Duration = TimeSpan.Zero,
							Name = "Second Task",
							FinishDate = DateTime.Now,
						}
					}
				},
				new() {
					Name = "Second Long Task",
					StartDate = DateTime.Now,
					EndDate = DateTime.Now + TimeSpan.FromDays(1),
					MaxDuration = TimeSpan.Zero,
					MaxShortTaskCount = 0,

					RunningTasks = new List<ShortTask>
					{
						new() {
							Duration = TimeSpan.Zero,
							Name = "Second Task",
							FinishDate = DateTime.Now,
						}
					}
				}
			};
		}
	}
}
