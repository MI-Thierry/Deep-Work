using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using System;

namespace DeepWork.ViewModels.Pages;

public partial class ShortTaskViewModel : ObservableObject
{
	public int Id { get; private set; }

	[ObservableProperty]
	private TimeSpan _duration = TimeSpan.Zero;

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private DateTimeOffset _finishDate = DateTime.Now;

	public static implicit operator ShortTaskViewModel(ShortTask model)
	{
		return new ShortTaskViewModel
		{
			Id = model.Id,
			Name = model.Name,
			Duration = model.Duration,
			FinishDate = model.FinishDate
		};
	}

	public static implicit operator ShortTask(ShortTaskViewModel viewModel)
	{
		return new ShortTask
		{
			Id = viewModel.Id,
			Name = viewModel.Name,
			Duration = viewModel.Duration,
			FinishDate = viewModel.FinishDate
		};
	}
}
