using DeepWork.Helpers;
using DeepWork.Models;
using DeepWork.Themes;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.ViewModels.Pages
{
	public class WeekPivotItemViewModel(IEnumerable<LongTask> longTasks, GraphTheme theme) : PivotItemViewModel(longTasks, theme, longTasks.Any() ? longTasks.First() : null)
	{
		public override void PlotTaskData()
		{
			DateTimeOffset firstDayOfWeek = Date.FirstDayOfWeek();
			DateTimeOffset lastDayOfWeek = Date.LastDayOfWeek();

			List<float?> weeklyData = [];
			List<float?> allWeeklyData = [];
			List<string> weeklyXAxisLabels = [];

			for (var date = firstDayOfWeek; date <= lastDayOfWeek; date = date.AddDays(1))
			{
				// Loading single task data.
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

				// Loading all tasks.
				values = 
					from longTask in LongTasks
					from task in longTask.FinishedTasks
					where DateOnly.FromDateTime(task.FinishDate.DateTime) == DateOnly.FromDateTime(date.DateTime)
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					allWeeklyData.Add((float)values.Sum());
				else
					allWeeklyData.Add(null);
			}

			TaskTotalDuration = TimeSpan.FromMinutes(weeklyData.Sum() ?? 0);
			AllTaskTotalDuration = TimeSpan.FromMinutes(allWeeklyData.Sum() ?? 0);
			TaskDurationPercentage = TaskTotalDuration.TotalMinutes * 100 / AllTaskTotalDuration.TotalMinutes;
			if (double.IsNaN(TaskDurationPercentage) || double.IsInfinity(TaskDurationPercentage))
				TaskDurationPercentage = 0;

			Series = [GenerateLineSeries(weeklyData, "Duration")];
			XAxis = [GenerateXAxis(weeklyXAxisLabels, "Days of week")];
			YAxis = [GenerateYAxis()];
		}
	}
}
