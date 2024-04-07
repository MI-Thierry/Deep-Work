using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Helpers;
using DeepWork.Models;
using DeepWork.Themes;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DeepWork.ViewModels.Pages
{
	public abstract partial class PivotItemViewModel : ObservableObject
	{
		protected IEnumerable<LongTask> LongTasks { get; set; }

		protected LongTask TaskToPlot { get; set; }

		public GraphTheme Theme { get; set; }

		[ObservableProperty]
		private DateTimeOffset _date = DateTimeOffset.Now;

		[ObservableProperty]
		private double _taskDurationPercentage;

		[ObservableProperty]
		private TimeSpan _allTaskTotalDuration;

		[ObservableProperty]
		private TimeSpan _taskTotalDuration;

		[ObservableProperty]
		ObservableCollection<ISeries> _series;

		[ObservableProperty]
		ICartesianAxis[] _xAxis = [];

		[ObservableProperty]
		ICartesianAxis[] _yAxis = [];

		public PivotItemViewModel(IEnumerable<LongTask> longTasks, GraphTheme theme, LongTask taskToPlot)
		{
			LongTasks = longTasks;
			TaskToPlot = taskToPlot;
			Theme = theme;
			PlotTaskData();
			this.PropertyChanged += PivotItemViewModel_PropertyChanged;
		}

		private void PivotItemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Date))
				ReloadGraph();
		}

		public abstract void PlotTaskData();

		public void ReloadGraph()
		{
			PlotTaskData();
		}

		public void ChangeTheme(GraphTheme theme)
		{
			Theme = theme;
			ReloadGraph();
		}

		public void ChangeTask(LongTask task)
		{
			TaskToPlot = task;
			ReloadGraph();
		}

		private string LabelFormatter(ChartPoint point)
		{
			TimeSpanToStringConverter converter = new();
			return (string)converter.Convert(TimeSpan.FromMinutes(point.Coordinate.PrimaryValue), default, default, default);
		}

		protected LineSeries<float?> GenerateLineSeries(IEnumerable<float?> data, string name)
		{

			return new LineSeries<float?>
			{
				YToolTipLabelFormatter = LabelFormatter,
				Values = data,
				Name = name,
				Stroke = new SolidColorPaint(Theme.StrokeColor, 2),
				GeometryStroke = new SolidColorPaint(Theme.StrokeColor, 2),
				Fill = new SolidColorPaint(Theme.FillColor)
			};
		}

		// Todo: Add GenerateBarSeries function.
		protected Axis GenerateXAxis(IEnumerable<string> labels, string name)
		{
			return new Axis
			{
				Name = name,
				Labels = [.. labels],
				LabelsRotation = -45,
				NamePaint = new SolidColorPaint(Theme.NamesForegroundColor),
				LabelsPaint = new SolidColorPaint(Theme.LabelsForegroundColor),
			};
		}

		protected Axis GenerateYAxis()
		{
			return new Axis
			{
				ShowSeparatorLines = true,
				NamePaint = new SolidColorPaint(Theme.NamesForegroundColor),
				LabelsPaint = new SolidColorPaint(Theme.LabelsForegroundColor),
				SeparatorsPaint = new SolidColorPaint(Theme.SeparatorForegroundColor, 1),
				SubseparatorsPaint = new SolidColorPaint
				{
					Color = Theme.SubSeparatorForegroundColor,
					StrokeThickness = 0.5f,
					PathEffect = new DashEffect([3, 3])
				},
				TicksPaint = new SolidColorPaint(Theme.TicksForegroundColor, 1.5f),
				SubticksPaint = new SolidColorPaint(Theme.SubTicksForegroundColor, 1)
			};
		}
	}
}
