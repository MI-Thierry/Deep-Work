using DeepWork.Helpers;
using DeepWork.Models;
using DeepWork.Themes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.ViewModels.Pages
{
	public class MonthPivotItemViewModel(IEnumerable<LongTask> longTasks, GraphTheme theme) : PivotItemViewModel(longTasks, theme, longTasks.Any() ? longTasks.First() : null)
	{
		public override void PlotTaskData()
		{
			DateTimeOffset firstDayOfMonth = Date.FirstDayOfMonth();
			DateTimeOffset lastDayOfMonth = Date.LastDayOfMonth();

			List<float?> monthlyData = [];
			List<float?> allMonthlyData = [];
			List<string> MonthlyXAxisLabels = [];

			for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
			{
				// Loading single task data.
				IEnumerable<double> values =
					from task in TaskToPlot.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					monthlyData.Add((float)values.Sum());
				else
					monthlyData.Add(null);

				MonthlyXAxisLabels.Add(date.ToString("dd/MMM"));

				// Loading all tasks.
				values =
					from longTask in LongTasks
					from task in longTask.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					allMonthlyData.Add((float)values.Sum());
				else
					allMonthlyData.Add(null);
			}

			TaskTotalDuration = TimeSpan.FromMinutes(monthlyData.Sum() ?? 0);
			AllTaskTotalDuration = TimeSpan.FromMinutes(allMonthlyData.Sum() ?? 0);
			TaskDurationPercentage = TaskTotalDuration.TotalMinutes * 100 / AllTaskTotalDuration.TotalMinutes;
			if (double.IsNaN(TaskDurationPercentage))
				TaskDurationPercentage = 0;

			Series = [GenerateLineSeries(monthlyData, "Duration")];
			XAxis = [GenerateXAxis(MonthlyXAxisLabels, "Days")];
			YAxis = [GenerateYAxis()];
		}
	}
}
