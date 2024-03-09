using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using System;

namespace DeepWork.ViewModels.Pages
{
	public partial class LongTaskViewModel : ObservableObject
	{
		[ObservableProperty]
		private int _taskCount;

		[ObservableProperty]
		private string _name = string.Empty;

		[ObservableProperty]
		private TimeSpan _maxDuration = TimeSpan.Zero;

		[ObservableProperty]
		private uint _maxShortTaskCount = 0;

		[ObservableProperty]
		DateTime _startDate = DateTime.Now;

		[ObservableProperty]
		DateTime _endDate = DateTime.Now + TimeSpan.FromDays(1);

		public static implicit operator LongTaskViewModel(LongTask longTask)
		{
			return new LongTaskViewModel
			{
				Name = longTask.Name,
				MaxDuration = longTask.MaxDuration,
				MaxShortTaskCount = longTask.MaxShortTaskCount,
				StartDate = longTask.StartDate,
				EndDate = longTask.EndDate,
				TaskCount = longTask.RunningTasks.Count
			};
		}

		public static implicit operator LongTask(LongTaskViewModel viewModel)
		{
			return new LongTask
			{
				Name = viewModel.Name,
				MaxDuration = viewModel.MaxDuration,
				MaxShortTaskCount = viewModel.MaxShortTaskCount,
				StartDate = viewModel.StartDate,
				EndDate = viewModel.EndDate,
			};
		}

		public string ConvertDateTimeToString(DateTime date) => date.ToString("ddd, MMM dd, yyyy");
	}
}
