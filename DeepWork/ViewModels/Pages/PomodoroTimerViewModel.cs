using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.Models;
using DeepWork.Services;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeepWork.ViewModels.Pages
{
	public partial class PomodoroTimerViewModel : ObservableObject
	{
		private readonly AccountManagementService _accountManager;
		private DispatcherQueueTimer _dispatcherQueueTimer;
		private TimeSpan _leftTime;

		public event PmdrPeriodEndedEventHandle PeriodEnded;
		public event PmdrWholeSessionEndedEventHandle WholePomodoroSessionEnded;
		public DispatcherQueue DispatcherQueue { get; private set; }

		[ObservableProperty]
		private TimeSpan _elapsedTime;

		[ObservableProperty]
		private TimeSpan _countDown;

		private TimeSpan _pmdrSessionDuration;
		public TimeSpan PmdrSessionDuration
		{
			get => _pmdrSessionDuration;
			set {
				SetProperty(ref _pmdrSessionDuration, value);
				PmdrSessionEndTime = DateTimeOffset.Now + _pmdrSessionDuration;
				TotalFocusPeriodCount = (int)_pmdrSessionDuration.TotalMinutes / 25;
				TotalBreakPeriodCount = Math.Max(TotalFocusPeriodCount - 1, 0);
			}
		}

		[ObservableProperty]
		private PeriodType _currentPeriodType;

		[ObservableProperty]
		private PeriodType _nextPeriodType;

		[ObservableProperty]
		private int _totalFocusPeriodCount;

		[ObservableProperty]
		private int _totalBreakPeriodCount;

		[ObservableProperty]
		private int _currentBreakPeriodCount;

		[ObservableProperty]
		private int _currentFocusPeriodCount;

		[ObservableProperty]
		private DateTimeOffset _pmdrSessionEndTime = DateTimeOffset.Now;

		[ObservableProperty]
		private TimeSpan _dailyTarget;

		[ObservableProperty]
		private TimeSpan _completedDailyTarget;

		[ObservableProperty]
		private ObservableCollection<LongTaskViewModel> _longTasks;

		[ObservableProperty]
		private ObservableCollection<ShortTaskViewModel> _shortTasks;

		public LongTaskViewModel SelectedLongTask { get; private set; }

		public ShortTaskViewModel SelectedShortTask { get; private set; }

		public PomodoroTimerViewModel(AccountManagementService accountManager)
		{
			_accountManager = accountManager;
			_accountManager.ActiveAccountChanged += LoadTasks;
			_longTasks = [];
			_shortTasks = [];
			LoadTasks(_accountManager.ActiveAccount);
		}

		private void LoadTasks(Account activeAccount)
		{
			if (activeAccount != null)
			{
				foreach (var task in activeAccount.RunningLongTasks)
					LongTasks.Add(task);

				if (LongTasks.Any())
					SelectLongTask(LongTasks.First().Id);
				DailyTarget = activeAccount.DailyTarget;
				CompletedDailyTarget = activeAccount.CompletedDailyTarget;
			}
		}

		public bool SelectLongTask(int id)
		{
			if (CurrentPeriodType is PeriodType.None)
			{
				SelectedLongTask = LongTasks.First(task => task.Id == id);

				ShortTasks.Clear();
				foreach (var task in _accountManager.GetLongTaskById(SelectedLongTask.Id).RunningTasks)
					ShortTasks.Add(task);
				return true;
			}
			return false;
		}

		public bool SelectShortTask(int id)
		{
			if (CurrentPeriodType is PeriodType.None)
			{
				SelectedShortTask = ShortTasks.First(task => task.Id == id);
				return true;
			}
			return false;
		}

		private void EditShortTask(ShortTaskViewModel editedShortTask)
		{
			_accountManager.EditShortTask(SelectedLongTask.Id, editedShortTask.Id, editedShortTask);
		}

		[RelayCommand]
		private void StartPomodoroSession(DispatcherQueue dispatcherQueue)
		{
			DispatcherQueue = dispatcherQueue;
			_dispatcherQueueTimer = DispatcherQueue.CreateTimer();
			_dispatcherQueueTimer.Interval = TimeSpan.FromSeconds(1);
			_dispatcherQueueTimer.IsRepeating = true;
			_dispatcherQueueTimer.Tick += DispatcherQueueTimer_Tick;

			CurrentFocusPeriodCount = 0;
			CurrentBreakPeriodCount = 0;
			NextPeriodType = PeriodType.FocusPeriod;
			ElapsedTime = TimeSpan.Zero;
			CountDown = TimeSpan.Zero;
			_leftTime = PmdrSessionDuration;
			_dispatcherQueueTimer.Start();
		}

		private void DispatcherQueueTimer_Tick(DispatcherQueueTimer sender, object args)
		{
			if (CountDown != TimeSpan.Zero)
			{
				CountDown = CountDown.Subtract(TimeSpan.FromSeconds(1));
				_leftTime = _leftTime.Subtract(TimeSpan.FromSeconds(1));
				ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
			}
			else
			{
				if (NextPeriodType == PeriodType.FocusPeriod)
				{
					CountDown = TimeSpan.FromMinutes(25);
					CurrentFocusPeriodCount++;
					PeriodEnded?.Invoke(CurrentPeriodType, NextPeriodType);
					CurrentPeriodType = NextPeriodType;
					NextPeriodType = CurrentBreakPeriodCount != TotalBreakPeriodCount ? PeriodType.BreakPeriod : PeriodType.None;

					if (SelectedShortTask != null)
					{
						SelectedShortTask.Duration += TimeSpan.FromMinutes(25);
						EditShortTask(SelectedShortTask);
					}
				}
				else if (NextPeriodType == PeriodType.BreakPeriod)
				{
					CountDown = TimeSpan.FromMinutes(5);
					CurrentBreakPeriodCount++;
					PeriodEnded?.Invoke(CurrentPeriodType, NextPeriodType);
					CurrentPeriodType = NextPeriodType;
					NextPeriodType = PeriodType.FocusPeriod;
				}
				else
				{
					StopPomodoroSession();
				}
			}
		}

		[RelayCommand]
		public void StopPomodoroSession()
		{
			DispatcherQueue?.TryEnqueue(delegate
			{
				_dispatcherQueueTimer.Stop();
				WholePomodoroSessionEnded?.Invoke(ElapsedTime);
				CurrentPeriodType = PeriodType.None;
				CountDown = TimeSpan.Zero;

				CompletedDailyTarget += ElapsedTime;
				_accountManager.SetAccountCompletedDailyTarget(CompletedDailyTarget);
			});
		}

		[RelayCommand]
		public void SetDailyTarget(TimeSpan dailyTarget)
		{
			_accountManager.SetAccountDailyTarget(dailyTarget);
			DailyTarget = dailyTarget;
		}

		public delegate void PmdrPeriodEndedEventHandle(PeriodType lastPeriodType, PeriodType nextPeriodType);
		public delegate void PmdrWholeSessionEndedEventHandle(TimeSpan elapsedTime);
	}
	public enum PeriodType
	{
		None,
		FocusPeriod,
		BreakPeriod,
	}
}
