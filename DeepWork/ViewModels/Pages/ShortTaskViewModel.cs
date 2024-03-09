using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Models;
using System;

namespace DeepWork.ViewModels.Pages
{
	public partial class ShortTaskViewModel : ObservableObject
	{
		public static implicit operator ShortTaskViewModel(ShortTask viewModel)
		{
			return new ShortTaskViewModel
			{
				Name = viewModel.Name,
				Duration = viewModel.Duration,
				FinishDate = viewModel.FinishDate
			};
		}

		public static implicit operator ShortTask(ShortTaskViewModel viewModel)
		{
			return new ShortTask
			{
				Name = viewModel.Name,
				Duration = viewModel.Duration,
				FinishDate = viewModel.FinishDate
			};
		}

		[ObservableProperty]
		private TimeSpan _duration = TimeSpan.Zero;

		[ObservableProperty]
		private string _name = string.Empty;

		[ObservableProperty]
		private DateTime _finishDate = DateTime.Now;
	}
}
