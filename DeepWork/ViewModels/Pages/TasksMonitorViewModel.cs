using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Helpers;
using DeepWork.Models;
using DeepWork.Services;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using Microsoft.UI.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeepWork.ViewModels.Pages
{
	public partial class TasksMonitorViewModel : ObservableObject
	{
		private readonly AccountManagementService _accountManager;
		private SKColor _labelsPaint;
		private SKColor _namesPaint;
		private SKColor _separatorsPaint;
		private SKColor _subSeparatorPaint;
		private SKColor _fillPaint;
		private SKColor _strokePaint;
		private SKColor _ticksPaint;
		private SKColor _subTicksPaint;

		public LongTask SelectedLongTask { get; set; }
		
		private DateTimeOffset _date;
		public DateTimeOffset Date
		{
			get => _date;
			set {
				SetProperty(ref _date, value);
				LoadData();
			}
		}

		[ObservableProperty]
		private ObservableCollection<ISeries> _weeklySeries;

		[ObservableProperty]
		private ICartesianAxis[] _weeklyXAxis;

		[ObservableProperty]
		private ICartesianAxis[] _weeklyYAxis;

		[ObservableProperty]
		private ObservableCollection<ISeries> _monthlySeries;

		[ObservableProperty]
		private ICartesianAxis[] _monthlyXAxis;

		[ObservableProperty]
		private ICartesianAxis[] _monthlyYAxis;

		[ObservableProperty]
		private ObservableCollection<ISeries> _yearlySeries;

		[ObservableProperty]
		private ICartesianAxis[] _yearlyXAxis;

		[ObservableProperty]
		private ICartesianAxis[] _yearlyYAxis;

		[ObservableProperty]
		private ObservableCollection<LongTaskViewModel> _longTasks;

		public TasksMonitorViewModel()
		{
			_accountManager = App.GetService<AccountManagementService>();
			_date = DateTimeOffset.Now;
			_longTasks = [];
			SelectedLongTask = _accountManager.ActiveAccount.RunningLongTasks[0];

			foreach (var task in _accountManager.ActiveAccount.RunningLongTasks)
				_longTasks.Add(task);

			LoadData();
		}

		public void SetTheme(ElementTheme theme)
		{
			if (theme == ElementTheme.Default)
				theme = Application.Current.RequestedTheme == ApplicationTheme.Light
					? ElementTheme.Light : ElementTheme.Dark;

			switch (theme)
			{
				case ElementTheme.Light:
					_labelsPaint = new(0xFF3D3D3D);
					_namesPaint = new(0xFF5E5E5E);
					_separatorsPaint = _labelsPaint;
					_subSeparatorPaint = new(0xFF5A5A5A);
					_fillPaint = new(0x59008AFF);
					_strokePaint = new(0xEDFF9605);
					_ticksPaint = _labelsPaint;
					_subTicksPaint = _labelsPaint;
					break;

				case ElementTheme.Dark:
					_labelsPaint = new(0xFFC3C3C3);
					_namesPaint = new(0xFFA0A0A0);
					_separatorsPaint = _labelsPaint;
					_subSeparatorPaint = new(0xFF5A5A5A);
					_fillPaint = new(0x59008AFF);
					_strokePaint = new(0xEDFF9605);
					_ticksPaint = _labelsPaint;
					_subTicksPaint = _labelsPaint;
					break;
				default:
					break;
			}
			LoadData();
		}
		public void SelectTask(LongTaskViewModel viewModel)
		{
			SelectedLongTask = _accountManager.GetLongTaskById(viewModel.Id);
			LoadData();
		}

		private void LoadData()
		{
			LoadWeeklyData();
			LoadMonthlyData();
			LoadYearlyData();
		}

		private void LoadWeeklyData()
		{
			DateTimeOffset firstDayOfWeek = Date.FirstDayOfWeek();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfWeek();

			List<float?> weeklyData = [];
			List<string> weeklyXAxisLabels = [];

			for (var date = firstDayOfWeek; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				IEnumerable<double> values =
					from task in SelectedLongTask.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(SelectedLongTask.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					weeklyData.Add((float)values.Sum());
				else
					weeklyData.Add(null);

				weeklyXAxisLabels.Add(date.ToString("ddd"));
			}

			WeeklySeries = [new LineSeries<float?> {
				Values = weeklyData.AsEnumerable(),
				Name = "Duration",
				Stroke = new SolidColorPaint(_strokePaint, 2),
				GeometryStroke = new SolidColorPaint(_strokePaint, 2),
				Fill = new SolidColorPaint(_fillPaint)
			}];

			WeeklyXAxis = [new Axis {
				Name = "Days of week",
				Labels = weeklyXAxisLabels,
				LabelsRotation = -45,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
			}];

			WeeklyYAxis = [new Axis {
				ShowSeparatorLines = true,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
				SeparatorsPaint = new SolidColorPaint(_separatorsPaint, 1),
				SubseparatorsPaint = new SolidColorPaint
				{
					Color = _subSeparatorPaint,
					StrokeThickness = 0.5f,
					PathEffect = new DashEffect([3, 3])
				},
				TicksPaint = new SolidColorPaint(_ticksPaint, 1.5f),
				SubticksPaint = new SolidColorPaint(_subTicksPaint, 1)
			}];
		}
		private void LoadMonthlyData()
		{
			DateTimeOffset firstDayOfMonth = Date.FirstDayOfMonth();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfMonth();

			List<float?> monthlyData = [];
			List<string> monthlyXAxisLabels = [];

			for (var date = firstDayOfMonth; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				IEnumerable<double> values =
					from task in SelectedLongTask.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(SelectedLongTask.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					monthlyData.Add((float)values.Sum());
				else
					monthlyData.Add(null);
				monthlyXAxisLabels.Add(date.ToString("dd/MMM"));
			}

			MonthlySeries = [new LineSeries<float?> {
				Values = monthlyData.AsEnumerable(),
				Name = "Duration",
				Stroke = new SolidColorPaint(_strokePaint, 2),
				GeometryStroke = new SolidColorPaint(_strokePaint, 2),
				Fill = new SolidColorPaint(_fillPaint)
			}];

			MonthlyXAxis = [new Axis {
				Name = "Days of month",
				Labels = monthlyXAxisLabels,
				LabelsRotation = -45,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
			}];

			MonthlyYAxis = [new Axis {
				ShowSeparatorLines = true,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
				SeparatorsPaint = new SolidColorPaint(_separatorsPaint, 1),
				SubseparatorsPaint = new SolidColorPaint
				{
					Color = _subSeparatorPaint,
					StrokeThickness = 0.5f,
					PathEffect = new DashEffect([3, 3])
				},
				TicksPaint = new SolidColorPaint(_ticksPaint, 1.5f),
				SubticksPaint = new SolidColorPaint(_subTicksPaint, 1)
			}];
		}
		private void LoadYearlyData()
		{
			DateTimeOffset firstDayOfYear = Date.FirstDayOfYear();
			DateTimeOffset lastDayOfYear = Date.LastDayOfYear();

			List<float?> yearlyData = [];
			List<string> yearlyXAxisLabels = [];

			for (var date = firstDayOfYear; date <= lastDayOfYear; date = date.AddMonths(1))
			{
				IEnumerable<double> values =
					from task in SelectedLongTask.FinishedTasks
					where task.FinishDate.Month == date.Month && task.FinishDate.Year == date.Year
					select task.Duration.TotalMinutes;

				if (SelectedLongTask.StartDate.Month <= date.Month
					&& SelectedLongTask.StartDate.Year <= date.Year
					&& date.Month <= DateTime.Now.Month
					&& date.Year <= DateTime.Now.Year)
					yearlyData.Add((float)values.Sum());
				else
					yearlyData.Add(null);
				yearlyXAxisLabels.Add(date.ToString("MMM/yyyy"));
			}

			YearlySeries = [new ColumnSeries<float?> {
				Values = yearlyData.AsEnumerable(),
				Name = "Duration",
				Stroke = new SolidColorPaint(_strokePaint, 2),
				Fill = new SolidColorPaint(_fillPaint)
			}];

			YearlyXAxis = [new Axis {
				Name = "Months",
				Labels = yearlyXAxisLabels,
				LabelsRotation = -45,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
			}];

			YearlyYAxis = [new Axis {
				ShowSeparatorLines = true,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
				SeparatorsPaint = new SolidColorPaint(_separatorsPaint, 1),
				SubseparatorsPaint = new SolidColorPaint
				{
					Color = _subSeparatorPaint,
					StrokeThickness = 0.5f,
					PathEffect = new DashEffect([3, 3])
				},
				TicksPaint = new SolidColorPaint(_ticksPaint, 1.5f),
				SubticksPaint = new SolidColorPaint(_subTicksPaint, 1)
			}];
		}
	}
}
