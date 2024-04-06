﻿using DeepWork.Helpers;
using DeepWork.Models;
using DeepWork.Themes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.ViewModels.Pages
{
	public class YearPivotItemViewModel(IEnumerable<LongTask> longTasks, GraphTheme theme) : PivotItemViewModel(longTasks, theme, longTasks.Any() ? longTasks.First() : null)
	{
		public override void PlotTaskData()
		{
			DateTimeOffset firstDayOfYear = Date.FirstDayOfYear();
			DateTimeOffset lastDayOfYear = Date.LastDayOfYear();

			List<float?> yearlyData = [];
			List<float?> allYearlyData = [];
			List<string> yearlyXAxisLabels = [];

			for (var date = firstDayOfYear; date <= lastDayOfYear; date = date.AddMonths(1))
			{
				IEnumerable<double> values =
					from task in TaskToPlot.FinishedTasks
					where task.FinishDate.Month == date.Month
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					yearlyData.Add((float)values.Sum());
				else
					yearlyData.Add(null);

				yearlyXAxisLabels.Add(date.ToString("MMM/yyyy"));

				// Loading all tasks.
				values =
					from longTask in LongTasks
					from task in longTask.FinishedTasks
					where task.FinishDate.Month == date.Month
					select task.Duration.TotalMinutes;

				if (DateOnly.FromDateTime(TaskToPlot.StartDate.DateTime) <= DateOnly.FromDateTime(date.DateTime) &&
					DateOnly.FromDateTime(date.DateTime) <= DateOnly.FromDateTime(DateTime.Now))
					allYearlyData.Add((float)values.Sum());
				else
					allYearlyData.Add(null);
			}

			TaskTotalDuration = TimeSpan.FromMinutes(yearlyData.Sum() ?? 0);
			AllTaskTotalDuration = TimeSpan.FromMinutes(allYearlyData.Sum() ?? 0);
			TaskDurationPercentage = TaskTotalDuration.TotalMinutes * 100 / AllTaskTotalDuration.TotalMinutes;
			if (double.IsNaN(TaskDurationPercentage))
				TaskDurationPercentage = 0;

			Series = [GenerateLineSeries(yearlyData, "Duration")];
			XAxis = [GenerateXAxis(yearlyXAxisLabels, "Days of week")];
			YAxis = [GenerateYAxis()];
		}
	}
}
