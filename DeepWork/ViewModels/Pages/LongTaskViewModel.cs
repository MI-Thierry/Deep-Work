using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using System;

namespace DeepWork.ViewModels.Pages;

public partial class LongTaskViewModel : ObservableObject
{
	public int Id { get; private set; }

	[ObservableProperty]
	private int _taskCount;

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private TimeSpan _maxDuration = TimeSpan.Zero;

	[ObservableProperty]
	private uint _maxShortTaskCount = 0;

	[ObservableProperty]
	DateTimeOffset _startDate = DateTimeOffset.Now;

	[ObservableProperty]
	DateTimeOffset _endDate = DateTimeOffset.Now + TimeSpan.FromDays(1);

	public static implicit operator LongTaskViewModel(LongTask model)
	{
		return new LongTaskViewModel
		{
			Id = model.Id,
			Name = model.Name,
			MaxDuration = model.MaxDuration,
			MaxShortTaskCount = model.MaxShortTaskCount,
			StartDate = model.StartDate,
			EndDate = model.EndDate,
			TaskCount = model.RunningTasks.Count
		};
	}

	public static implicit operator LongTask(LongTaskViewModel viewModel)
	{
		return new LongTask
		{
			Id = viewModel.Id,
			Name = viewModel.Name,
			MaxDuration = viewModel.MaxDuration,
			MaxShortTaskCount = viewModel.MaxShortTaskCount,
			StartDate = viewModel.StartDate,
			EndDate = viewModel.EndDate,
		};
	}

	public string ConvertDateTimeToString(DateTimeOffset date) => date.ToString("ddd, MMM dd, yyyy");
}
