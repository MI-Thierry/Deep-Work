using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
		private bool _isAllTasks;

		
		private DateTimeOffset _date = DateTimeOffset.Now;
		public DateTimeOffset Date
		{
			get => _date;
			set {
				SetProperty(ref _date, value);
				PlotData();
			}
		}

		[ObservableProperty]
		private string _plottingStatusMessage = "There's no available data to plot on the graph.";

		[ObservableProperty]
		private float _maxWeeklyHours;

		[ObservableProperty]
		private float _maxMonthlyHours;

		[ObservableProperty]
		private float _maxYearlyHours;

		[ObservableProperty]
		private LongTask _taskToPlot = null;

		[ObservableProperty]
		private ObservableCollection<ISeries> _weeklySeries = [];

		[ObservableProperty]
		ICartesianAxis[] _weeklyXAxis = [];

		[ObservableProperty]
		ICartesianAxis[] _weeklyYAxis = [];

		[ObservableProperty]
		private ObservableCollection<ISeries> _monthlySeries = [];

		[ObservableProperty]
		private ICartesianAxis[] _monthlyXAxis = [];

		[ObservableProperty]
		private ICartesianAxis[] _monthlyYAxis = [];

		[ObservableProperty]
		private ObservableCollection<ISeries> _yearlySeries = [];

		[ObservableProperty]
		private ICartesianAxis[] _yearlyXAxis = [];
		[ObservableProperty]
		private ICartesianAxis[] _yearlyYAxis = [];

		[ObservableProperty]
		private ObservableCollection<LongTaskViewModel> _longTasks = [];

		public TasksMonitorViewModel(ElementTheme theme)
		{
			_accountManager = App.GetService<AccountManagementService>();

			if (_accountManager.ActiveAccount.RunningLongTasks.Count != 0)
			{
				_taskToPlot = _accountManager.ActiveAccount.RunningLongTasks.First();
				foreach (var task in _accountManager.ActiveAccount.RunningLongTasks)
					_longTasks.Add(task);
				SetTheme(theme);
				PlotData();
				_plottingStatusMessage = null;
			}
		}

		[RelayCommand]
		private void PlotAllTasksData()
		{
			_isAllTasks = true;
			PlotAllTasksWeeklyData();
			PlotAllTasksMonthlyData();
			PlotAllTasksYearlyData();
		}

		[RelayCommand]
		public void ChangeTheme(ElementTheme theme)
		{
			SetTheme(theme);
			if (_isAllTasks)
				PlotAllTasksData();
			else
				PlotData();
		}

		public void PlotTask(LongTaskViewModel viewModel)
		{
			TaskToPlot = _accountManager.GetLongTaskById(viewModel.Id);
			PlotData();
		}

		private void SetTheme(ElementTheme theme)
		{
			if (theme == ElementTheme.Default)
				theme = Application.Current.RequestedTheme == ApplicationTheme.Light
					? ElementTheme.Light : ElementTheme.Dark;

			switch (theme)
			{
				case ElementTheme.Light:
					_labelsPaint = new(0xff3d3d3d);
					_namesPaint = new(0xff5e5e5e);
					_separatorsPaint = _labelsPaint;
					_subSeparatorPaint = new(0xff5a5a5a);
					_fillPaint = new(0x59008aff);
					_strokePaint = new(0xedff9605);
					_ticksPaint = _labelsPaint;
					_subTicksPaint = _labelsPaint;
					break;

				case ElementTheme.Dark:
				default:
					_labelsPaint = new(0xffc3c3c3);
					_namesPaint = new(0xffa0a0a0);
					_separatorsPaint = _labelsPaint;
					_subSeparatorPaint = new(0xff5a5a5a);
					_fillPaint = new(0x59008aff);
					_strokePaint = new(0xedff9605);
					_ticksPaint = _labelsPaint;
					_subTicksPaint = _labelsPaint;
					break;
			}
		}

		private void PlotData()
		{
			PlotWeeklyData();
			PlotMonthlyData();
			PlotYearlyData();
		}

		private void PlotAllTasksWeeklyData()
		{
			DateTimeOffset firstDayOfWeek = Date.FirstDayOfWeek();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfWeek();

			List<float?> weeklyData = [];
			List<string> weeklyXAxisLabels = [];

			for (var date = firstDayOfWeek; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				IEnumerable<float> values =
					from longTask in _accountManager.ActiveAccount.RunningLongTasks
					from task in longTask.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select (float)task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					weeklyData.Add(values.Sum());
				else
					weeklyData.Add(null);

				weeklyXAxisLabels.Add(date.ToString("ddd"));
			}

			MaxMonthlyHours = weeklyData.Max() ?? 0;
			WeeklySeries = [GenerateSeries(weeklyData, "Duration")];
			WeeklyXAxis = [GenerateXAxis(weeklyXAxisLabels, "Days of week")];
			WeeklyYAxis = [GenerateYAxis()];
		}

		private void PlotWeeklyData()
		{
			DateTimeOffset firstDayOfWeek = Date.FirstDayOfWeek();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfWeek();

			List<float?> weeklyData = [];
			List<string> weeklyXAxisLabels = [];

			for (var date = firstDayOfWeek; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				IEnumerable<double> values =
					from task in TaskToPlot.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					weeklyData.Add((float)values.Sum());
				else
					weeklyData.Add(null);

				weeklyXAxisLabels.Add(date.ToString("ddd"));
				MaxWeeklyHours = weeklyData.Max() ?? 0;
			}

			WeeklySeries = [GenerateSeries(weeklyData, "Duration")];
			WeeklyXAxis = [GenerateXAxis(weeklyXAxisLabels, "Days of week")];
			WeeklyYAxis = [GenerateYAxis()];
		}

		private void PlotAllTasksMonthlyData()
		{
			DateTimeOffset firstDayOfMonth = Date.FirstDayOfMonth();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfMonth();

			List<float?> monthlyData = [];
			List<string> monthlyXAxisLabels = [];

			for (var date = firstDayOfMonth; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				IEnumerable<double> values =
					from longTask in _accountManager.ActiveAccount.RunningLongTasks
					from task in longTask.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					monthlyData.Add((float)values.Sum());
				else
					monthlyData.Add(null);
				monthlyXAxisLabels.Add(date.ToString("dd/MMM"));
			}

			MaxMonthlyHours = monthlyData.Max() ?? 0;
			MonthlySeries = [GenerateSeries(monthlyData, "Duration")];
			MonthlyXAxis = [GenerateXAxis(monthlyXAxisLabels, "Days of Month")];
			MonthlyYAxis = [GenerateYAxis()];
		}

		private void PlotMonthlyData()
		{
			DateTimeOffset firstDayOfMonth = Date.FirstDayOfMonth();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfMonth();

			List<float?> monthlyData = [];
			List<string> monthlyXAxisLabels = [];

			for (var date = firstDayOfMonth; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				IEnumerable<double> values =
					from task in TaskToPlot.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					monthlyData.Add((float)values.Sum());
				else
					monthlyData.Add(null);
				monthlyXAxisLabels.Add(date.ToString("dd/MMM"));
			}

			MaxMonthlyHours = monthlyData.Max() ?? 0;
			MonthlySeries = [GenerateSeries(monthlyData, "Duration")];
			MonthlyXAxis = [GenerateXAxis(monthlyXAxisLabels, "Days of Month")];
			MonthlyYAxis = [GenerateYAxis()];
		}

		private void PlotAllTasksYearlyData()
		{
			DateTimeOffset firstDayOfYear = Date.FirstDayOfYear();
			DateTimeOffset lastDayOfYear = Date.LastDayOfYear();

			List<float?> yearlyData = [];
			List<string> yearlyXAxisLabels = [];

			for (var date = firstDayOfYear; date <= lastDayOfYear; date = date.AddMonths(1))
			{
				IEnumerable<double> values =
					from longTask in _accountManager.ActiveAccount.RunningLongTasks
					from task in longTask.FinishedTasks
					where task.FinishDate.Month == date.Month && task.FinishDate.Year == date.Year
					select task.Duration.TotalMinutes;

				if (TaskToPlot.StartDate.Month <= date.Month
					&& TaskToPlot.StartDate.Year <= date.Year
					&& date.Month <= DateTime.Now.Month
					&& date.Year <= DateTime.Now.Year)
					yearlyData.Add((float)values.Sum());
				else
					yearlyData.Add(null);
				yearlyXAxisLabels.Add(date.ToString("MMM/yyyy"));
			}

			MaxYearlyHours = yearlyData.Max() ?? 0;
			YearlySeries = [GenerateSeries(yearlyData, "Duration")];
			YearlyXAxis = [GenerateXAxis(yearlyXAxisLabels, "Months of Year")];
			YearlyYAxis = [GenerateYAxis()];
		}

		private void PlotYearlyData()
		{
			DateTimeOffset firstDayOfYear = Date.FirstDayOfYear();
			DateTimeOffset lastDayOfYear = Date.LastDayOfYear();

			List<float?> yearlyData = [];
			List<string> yearlyXAxisLabels = [];

			for (var date = firstDayOfYear; date <= lastDayOfYear; date = date.AddMonths(1))
			{
				IEnumerable<double> values =
					from task in TaskToPlot.FinishedTasks
					where task.FinishDate.Month == date.Month && task.FinishDate.Year == date.Year
					select task.Duration.TotalMinutes;

				if (TaskToPlot.StartDate.Month <= date.Month
					&& TaskToPlot.StartDate.Year <= date.Year
					&& date.Month <= DateTime.Now.Month
					&& date.Year <= DateTime.Now.Year)
					yearlyData.Add((float)values.Sum());
				else
					yearlyData.Add(null);
				yearlyXAxisLabels.Add(date.ToString("MMM/yyyy"));
			}

			MaxYearlyHours = yearlyData.Max() ?? 0;
			YearlySeries = [GenerateSeries(yearlyData, "Duration")];
			YearlyXAxis = [GenerateXAxis(yearlyXAxisLabels, "Months of Year")];
			YearlyYAxis = [GenerateYAxis()];
		}

		private LineSeries<float?> GenerateSeries(IEnumerable<float?> data, string name)
		{
			return new LineSeries<float?>
			{
				Values = data,
				Name = name,
				Stroke = new SolidColorPaint(_strokePaint, 2),
				GeometryStroke = new SolidColorPaint(_strokePaint, 2),
				Fill = new SolidColorPaint(_fillPaint)
			};
		}

		private Axis GenerateXAxis(IEnumerable<string> labels, string name)
		{
			return new Axis
			{
				Name = name,
				Labels = [.. labels],
				LabelsRotation = -45,
				NamePaint = new SolidColorPaint(_namesPaint),
				LabelsPaint = new SolidColorPaint(_labelsPaint),
			};
		}

		private Axis GenerateYAxis()
		{
			return new Axis
			{
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
			};
		}
	}
}
