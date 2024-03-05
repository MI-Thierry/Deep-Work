using DeepWork.Models;
using DeepWork.Services;
using DeepWork.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace DeepWork.MVVM.Views
{
    public sealed partial class FocusSessionPage : Page
    {
        private AccountManagementServices m_AccountManager;
        private static ShortTask SelectedShortTask;
        private static LongTask SelectedLongTask;
        private ObservableCollection<LongTask> LongTasks { get; set; }
        private ObservableCollection<ShortTask> ShortTasks { get; set; }

        public static FocusSessionManager SessionManager { get; set; }
        static FocusSessionPage()
        {
            SessionManager = new FocusSessionManager();
        }

        public FocusSessionPage()
        {
            m_AccountManager = App._serviceProvider.GetRequiredService<AccountManagementServices>();
            LongTasks = m_AccountManager.UserAccount.LongTasks;
            ShortTasks = new ObservableCollection<ShortTask>();

            this.InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            SessionManager.WholeSessionEnded += SessionManager_WholeSessionEnded;
            SessionManager.PropertyChanged += SessionManager_PropertyChanged;
            SessionManager.SessionEnded += SessionManager_SessionEnded;

            UpdatePageContent(SessionManager.SessionType);
        }

        private void UpdatePageContent(SessionType sessionType)
        {
            if (sessionType == SessionType.None)
            {
                OptionsViewer.Visibility = Visibility.Visible;
                ProgressViewer.Visibility = Visibility.Collapsed;
            }
            else
            {
                ProgressViewer.Visibility = Visibility.Visible;
                OptionsViewer.Visibility = Visibility.Collapsed;
            }

            if (sessionType == SessionType.Focus)
            {
                NextSessionBlockText.Text = SessionManager.CurrentBreakCount != 0 ? "Up Next: 5 min Break" : "Up Next: End of session";
                SessionTypeTextBlock.Text = $"Focus Period({SessionManager.TotalFocusCount - SessionManager.CurrentFocusCount} of {SessionManager.TotalFocusCount})";
            }
            else if (sessionType == SessionType.Break)
            {
                NextSessionBlockText.Text = "Up Next: 30 min Focus";
                SessionTypeTextBlock.Text = $"Break Period({SessionManager.TotalFocusCount - SessionManager.CurrentBreakCount - 1} of {SessionManager.TotalBreakCount})";
            }

            InputBox.MinuteDisplay.TextChanged += MinuteDisplay_TextChanged;
            EndTimeTextBlock.Text = $"Focus Session until {(DateTime.Now + TimeSpan.FromMinutes(30)).ToShortTimeString()}";
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SessionManager.WholeSessionEnded -= SessionManager_WholeSessionEnded;
			SessionManager.PropertyChanged -= SessionManager_PropertyChanged;
            SessionManager.SessionEnded -= SessionManager_SessionEnded;
        }

        private void SessionManager_SessionEnded(SessionType type)
        {
            DispatcherQueue?.TryEnqueue(() =>
            {
                UpdatePageContent(
                    type == SessionType.Focus ?
                    SessionType.Break :
                    SessionType.Focus);
            });
        }


        private void SessionManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DispatcherQueue?.TryEnqueue(() =>
            {
                ElapsedTimeTextBlock.Text = $"Elapsed time is: {SessionManager.ElapsedTime}";
                MinutesTextBlock.Text = (SessionManager.CountDown > TimeSpan.FromMinutes(1)) ?
                    $"{SessionManager.CountDown.Minutes} Mins" :
                    $"{SessionManager.CountDown.Seconds} Secs";

                if (SessionManager.SessionType == SessionType.Focus)
                    UpdateSessionProgressRing(TimeSpan.FromMinutes(30), SessionManager.CountDown);

                else
                    UpdateSessionProgressRing(TimeSpan.FromMinutes(5), SessionManager.CountDown);
            });
        }

        private void UpdateSessionProgressRing(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            TimeSpan leftTime = totalTime - elapsedTime;
            TimeProgressRing.Value = leftTime.TotalSeconds * 100 / totalTime.TotalSeconds;
        }

        private void SessionManager_WholeSessionEnded(TimeSpan elapsedTime)
        {
            DispatcherQueue?.TryEnqueue(() =>
            {
                OptionsViewer.Visibility = Visibility.Visible;
                ProgressViewer.Visibility = Visibility.Collapsed;
                SelectedShortTask.Duration += elapsedTime;
                m_AccountManager.SaveChanges();
            });
        }

        private void MinuteDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeSpan duration = TimeSpan.FromMinutes(int.Parse((sender as TextBox).Text));
            int breaks = (int)(duration.TotalMinutes / 30) - 1;
            if (breaks != 0)
                BreakNumberTextBlock.Text = $"You'll have {breaks} breaks";
            else
                BreakNumberTextBlock.Text = "You'll have no breaks";

            EndTimeTextBlock.Text = $"Focus Session until {(DateTime.Now + duration).ToShortTimeString()}";
        }

        private void StartSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedShortTask != null)
            {
                double dur = double.Parse(InputBox.MinuteDisplay.Text);
                SessionManager.StartSession(TimeSpan.FromMinutes(dur));
                OptionsViewer.Visibility = Visibility.Collapsed;
                ProgressViewer.Visibility = Visibility.Visible;
            }
            else
            {
                Utils.Utils.WarningDialog("Select Task before Starting Focus Session.", Content.XamlRoot);
            }
        }

        private void StopSessionButton_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.StopFocusSession();
        }

        private void ContListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Tracking the change on Tasks in Continuous Tasks using this events
            if (e.RemovedItems.Count != 0)
                ((LongTask)e.RemovedItems[0]).RunningTasks.CollectionChanged -= Tasks_CollectionChanged;
            if (e.AddedItems.Count != 0)
            {
                ((LongTask)e.AddedItems[0]).RunningTasks.CollectionChanged += Tasks_CollectionChanged;
                SelectedLongTask = (LongTask)e.AddedItems[0];

                ShortTasks.Clear();
                foreach (var task in SelectedLongTask.RunningTasks)
                {
                    ShortTasks.Add(task);
                }
                TaskNameTextBlock.Text = ((LongTask)e.AddedItems[0]).Name;
            }
        }

        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ShortTasks.Clear();
            foreach (var task in (ObservableCollection<ShortTask>)sender)
            {
                ShortTasks.Add(task);
            }
        }

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Any())
                SelectedShortTask = e.AddedItems.First() as ShortTask;
            else
                SelectedShortTask = null;
        }
    }
}
