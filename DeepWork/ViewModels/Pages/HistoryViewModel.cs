using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Services;
using System.Collections.ObjectModel;

namespace DeepWork.ViewModels.Pages
{
	public partial class HistoryViewModel : ObservableObject
	{
		private readonly AccountManagementService _accountManager;

		[ObservableProperty]
		ObservableCollection<TaskViewModel> _runningTaskHistory = [];

		[ObservableProperty]
		ObservableCollection<TaskViewModel> _finishedTaskHistory = [];

		public HistoryViewModel()
		{
			_accountManager = App.GetService<AccountManagementService>();

			if (_accountManager.ActiveAccount.RunningLongTasks.Count > 0)
			{
				foreach (var longTask in _accountManager.ActiveAccount.RunningLongTasks)
				{
					TaskViewModel longTaskVm = new()
					{
						Type = TaskType.LongTask,
						Name = longTask.Name,
						FinishData = longTask.EndDate
					};
					foreach (var shortTask in longTask.FinishedTasks)
					{
						TaskViewModel shortTaskVm = new()
						{
							Type = TaskType.ShortTask,
							Name = shortTask.Name,
							FinishData = shortTask.FinishDate,
						};
						longTaskVm.Children.Add(shortTaskVm);
					}
					_runningTaskHistory.Add(longTaskVm);
				}
				foreach (var longTask in _accountManager.ActiveAccount.FinishedLongTasks)
				{
					TaskViewModel longTaskVm = new()
					{
						Type = TaskType.LongTask,
						Name = longTask.Name,
						FinishData = longTask.EndDate
					};
					foreach (var shortTask in longTask.FinishedTasks)
					{
						TaskViewModel shortTaskVm = new()
						{
							Type = TaskType.ShortTask,
							Name = shortTask.Name,
							FinishData = shortTask.FinishDate,
						};
						longTaskVm.Children.Add(shortTaskVm);
					}
					_finishedTaskHistory.Add(longTaskVm);
				}
			}
		}
	}
}
