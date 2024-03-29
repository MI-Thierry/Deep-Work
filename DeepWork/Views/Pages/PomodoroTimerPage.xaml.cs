using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Linq;

namespace DeepWork.Views.Pages
{
	public sealed partial class PomodoroTimerPage : Page
	{
		public PomodoroTimerViewModel ViewModel { get; set; }
		public TaskManagementViewModel TaskManagementViewModel { get; set; }

		public PomodoroTimerPage()
		{
			TaskManagementViewModel = new TaskManagementViewModel();
			ViewModel = App.GetService<PomodoroTimerViewModel>();
			ViewModel.PeriodEnded += ViewModel_PeriodEnded;
			ViewModel.WholePomodoroSessionEnded += ViewModel_WholePomodoroSessionEnded;
			DataContext = ViewModel;
			this.InitializeComponent();
		}

		private void ViewModel_WholePomodoroSessionEnded(TimeSpan elapsedTime)
		{
			AppNotificationBuilder builder = new();
			builder.AddText("Great Work.");
			builder.AddText($"You've finished pomodoro session of {Math.Round(elapsedTime.TotalMinutes)} minutes");

			AppNotificationManager notificationManager = AppNotificationManager.Default;
			notificationManager.Show(builder.BuildNotification());
		}

		private void ViewModel_PeriodEnded(PeriodType lastPeriodType, PeriodType nextPeriodType)
		{
			AppNotificationBuilder builder = new();
			if (nextPeriodType == PeriodType.FocusPeriod)
			{
				builder.AddText("Time to Focus.");
				builder.AddText("Now it's time to focus for 25 minutes");
			}
			else
			{
				builder.AddText("Time for break.");
				builder.AddText("Now it's time to take break for 5 minutes");
			}

			builder.AddButton(new AppNotificationButton("Stop Pomodoro Session")
				.AddArgument("action", "stopPomodoroSession"));
			builder.AddButton(new AppNotificationButton("Dismiss")
				.AddArgument("action", "dismiss"));

			AppNotificationManager notificationManager = AppNotificationManager.Default;
			notificationManager.Show(builder.BuildNotification());
		}

		public int GetMinutesFromTimeSpan(TimeSpan timeSpan) =>
			(int)timeSpan.TotalMinutes;
		public TimeSpan GetTimeSpanFromMinutes(int minutes) =>
			ViewModel.PmdrSessionDuration = TimeSpan.FromMinutes(minutes);

		public string ConvertDateTimeToString(DateTimeOffset dateTime) => dateTime.ToString("t");

		public string ConvertPeriodTypeToString(PeriodType periodType) =>
			periodType == PeriodType.FocusPeriod ? $"Focus Period"
			: periodType == PeriodType.BreakPeriod ? $"Break Period"
			: "Finish";

		public string ConvertTimeSpanToString(TimeSpan timeSpan) =>
			timeSpan > TimeSpan.FromMinutes(1) ? timeSpan.Minutes.ToString("00") + " Mins"
			: timeSpan.Seconds.ToString("00") + " Secs";

		public double CalculatePercentage(TimeSpan timeSpan, PeriodType periodType) =>
			periodType switch
			{
				PeriodType.FocusPeriod => timeSpan.TotalSeconds * (100 / TimeSpan.FromMinutes(25).TotalSeconds),
				PeriodType.BreakPeriod => timeSpan.TotalSeconds * (100 / TimeSpan.FromMinutes(5).TotalSeconds),
				_ => 0.0,
			};

		public Visibility ConvertPeriodTypeToVisibility(PeriodType periodType , string intendedPeriodType)
		{
			if (intendedPeriodType == "InSession" && periodType != PeriodType.None)
				return Visibility.Visible;
			else if (intendedPeriodType == "OutSession" && periodType == PeriodType.None)
				return Visibility.Visible;
			else
				return Visibility.Collapsed;
		}

		private void LongTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Any())
				ViewModel.SelectLongTask((e.AddedItems.First() as LongTaskViewModel).Id);
		}

		private void SelectShortTaskCheckBox_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.SelectShortTask((int)(sender as CheckBox).Tag);
		}
	}
}
