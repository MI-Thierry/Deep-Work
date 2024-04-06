using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using DeepWork.Services;
using DeepWork.Themes;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace DeepWork.ViewModels.Pages
{
	public partial class TasksMonitorViewModel : ObservableObject
	{
		private readonly AccountManagementService _accountManager;

		[ObservableProperty]
		private WeekPivotItemViewModel _weekViewModel = null;

		[ObservableProperty]
		private MonthPivotItemViewModel _monthViewModel = null;

		[ObservableProperty]
		private YearPivotItemViewModel _yearViewModel = null;

		[ObservableProperty]
		private ObservableCollection<LongTaskViewModel> _longTasks = [];

		public TasksMonitorViewModel(ElementTheme theme)
		{
			_accountManager = App.GetService<AccountManagementService>();
			if (_accountManager.IsAccountAvailable && _accountManager.ActiveAccount.RunningLongTasks.Count > 0)
			{
				GraphTheme graphTheme = (theme == ElementTheme.Light) ? GraphTheme.Light
					: (theme == ElementTheme.Dark) ? GraphTheme.Dark
					: GraphTheme.Default;

				_weekViewModel = new WeekPivotItemViewModel(_accountManager.ActiveAccount.RunningLongTasks, graphTheme);
				_monthViewModel = new MonthPivotItemViewModel(_accountManager.ActiveAccount.RunningLongTasks, graphTheme);
				_yearViewModel = new YearPivotItemViewModel(_accountManager.ActiveAccount.RunningLongTasks, graphTheme);
				foreach (var task in _accountManager.ActiveAccount.RunningLongTasks)
					_longTasks.Add(task);
			}
		}

		public void PlotTask(LongTaskViewModel taskToPlot)
		{
			LongTask task  = _accountManager.GetLongTaskById(taskToPlot.Id);
			WeekViewModel?.ChangeTask(task);
			MonthViewModel?.ChangeTask(task);
			YearViewModel?.ChangeTask(task);
		}

		public void ChangeTheme(ElementTheme theme)
		{
			GraphTheme graphTheme = (theme == ElementTheme.Light) ? GraphTheme.Light
					: (theme == ElementTheme.Dark) ? GraphTheme.Dark
					: GraphTheme.Default;

			WeekViewModel?.ChangeTheme(graphTheme);
			MonthViewModel?.ChangeTheme(graphTheme);
			YearViewModel?.ChangeTheme(graphTheme);
		}
	}
}
