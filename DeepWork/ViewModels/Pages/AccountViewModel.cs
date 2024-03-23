using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using Microsoft.UI.Xaml;
using System;

namespace DeepWork.ViewModels.Pages
{
	public partial class AccountViewModel : ObservableObject
	{
		public int Id { get; private set; }

		[ObservableProperty]
		private string _userName;

		[ObservableProperty]
		private bool _isActive;

		[ObservableProperty]
		private ElementTheme _theme;

		[ObservableProperty]
		private TimeSpan _dailyTarget;

		[ObservableProperty]
		private int _runningLongTasksCount;

		[ObservableProperty]
		private int _finishedLongTasksCount;

		public static implicit operator AccountViewModel(Account model)
		{
			return new AccountViewModel
			{
				Id = model.Id,
				UserName = model.Username,
				IsActive = model.IsActive,
				Theme = model.Theme,
				DailyTarget = model.DailyTarget,
				RunningLongTasksCount = model.RunningLongTasks.Count,
				FinishedLongTasksCount = model.FinishedLongTasks.Count
			};
		}
	}
}
