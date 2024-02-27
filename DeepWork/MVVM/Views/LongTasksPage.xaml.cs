using DeepWork.MVVM.Models;
using DeepWork.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DeepWork.MVVM.Views
{
    public sealed partial class LongTasksPage : Page
    {
        private readonly AccountManagementServices AccountManager;
        private ObservableCollection<LongTask> LongTasks { get; set; }
        private ObservableCollection<ShortTask> CurrentShortTasks { get; set; }
        private LongTask SelectedLongTask;
        public LongTasksPage()
        {
            // Getting long tasks from account management services
            AccountManager = App.ServiceProvider.GetRequiredService<AccountManagementServices>();
            LongTasks = AccountManager.UserAccount.LongTasks;
            CurrentShortTasks = new ObservableCollection<ShortTask>();

            this.InitializeComponent();
        }

        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            float fontSize = 15;
            StackPanel stackPanel = new() { Spacing = 10 };
            TextBlock taskNameText = new() { Text = "Task Name", FontSize = fontSize };
            stackPanel.Children.Add(taskNameText);
            TextBox taskName = new() { PlaceholderText = "Name", TextWrapping = TextWrapping.Wrap, FontSize = fontSize };
            stackPanel.Children.Add(taskName);
            NumberBox durBox = new()
            {
                ValidationMode = NumberBoxValidationMode.InvalidInputOverwritten,
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Hidden,
                PlaceholderText = "Duration",
            };
            stackPanel.Children.Add(durBox);
            ContentDialog TaskDialog = new()
            {
                Title = "Add Task To Continuous Task",
                Content = stackPanel,
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.Content.XamlRoot
            };
            ContentDialogResult contentDialogResult = await TaskDialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {

                AccountManager.AddShortTask(
                    (LongTask)LongTaskList.SelectedItem,
                    new ShortTask
                    {
                        Name = taskName.Text,
                        Duration = TimeSpan.FromMinutes(int.Parse(durBox.Text))
                    });
            }
        }

        private async void AddContButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountManager.IsAccountAvailable)
            {
                float fontSize = 15;
                StackPanel stackPanel = new() { Spacing = 10 };
                TextBlock startDateText = new() { Text = "Start Date", FontSize = fontSize };
                stackPanel.Children.Add(startDateText);
                DatePicker startDate = new() { Date = DateTime.Now };
                stackPanel.Children.Add(startDate);
                TextBlock endDateText = new() { Text = "End Date", FontSize = fontSize };
                stackPanel.Children.Add(endDateText);
                DatePicker endDate = new() { Date = DateTime.Now + TimeSpan.FromDays(1) };
                stackPanel.Children.Add(endDate);
                TextBlock taskNameText = new() { Text = "Task Name", FontSize = fontSize };
                stackPanel.Children.Add(taskNameText);
                TextBox taskName = new() { TextWrapping = TextWrapping.Wrap, FontSize = fontSize, PlaceholderText = "Name" };
                stackPanel.Children.Add(taskName);

                ContentDialog TaskDialog = new()
                {
                    Title = "Add Continuous Task",
                    Content = stackPanel,
                    PrimaryButtonText = "Save",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.Content.XamlRoot
                };
                ContentDialogResult contentDialogResult = await TaskDialog.ShowAsync();
                if (contentDialogResult == ContentDialogResult.Primary)
                {
                    AccountManager.AddLongTask(
                        new LongTask
                        {
                            Name = taskName.Text,
                            StartDate = startDate.Date.Date,
                            EndDate = endDate.Date.Date,
                        });
                }
            }
            else
            {
                Utils.Utils.WarningDialog("Create account first.", Content.XamlRoot);
            }
        }

        private async void WarningDialog(string message)
        {
            ContentDialog TaskDialog = new()
            {
                Title = "Warning",
                Content = message,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };
            _ = await TaskDialog.ShowAsync();
        }

        private void ClosePaneButton_Click(object sender, RoutedEventArgs e)
        {
            TaskViewer.Visibility = Visibility.Collapsed;
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

                TaskViewer.Visibility = Visibility.Visible;
                CurrentShortTasks.Clear();
                foreach (var task in SelectedLongTask.RunningTasks)
                {
                    CurrentShortTasks.Add(task);
                }
                TasksViewerHeader.Text = ((LongTask)e.AddedItems[0]).Name;
            }
        }
        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CurrentShortTasks.Clear();
            foreach (var task in (ObservableCollection<ShortTask>)sender)
            {
                CurrentShortTasks.Add(task);
            }
        }

        private void FinishTaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).IsChecked = false;
            AccountManager.FinishShortTask(SelectedLongTask ,(sender as CheckBox).Tag as string);
        }

        private void EndLongTaskCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).IsChecked = false;
            LongTaskList.SelectedItem = null;
            TaskViewer.Visibility = Visibility.Collapsed;
            AccountManager.FinishLongTask((sender as CheckBox).Tag as string);
        }

        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = new() { Spacing = 10 };
            TextBox taskNewName = new() { Text = (sender as Button).Tag as string };
            stackPanel.Children.Add(taskNewName);
            NumberBox durBox = new()
            {
                ValidationMode = NumberBoxValidationMode.InvalidInputOverwritten,
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Hidden,
                PlaceholderText = "Duration",
            };
            stackPanel.Children.Add(durBox);
            ContentDialog TaskDialog = new()
            {
                Title = "Add Continuous Task",
                Content = stackPanel,
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.Content.XamlRoot
            };
            ContentDialogResult contentDialogResult = await TaskDialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {
                ShortTask task = SelectedLongTask.RunningTasks.First(o => o.Name == (sender as Button).Tag as string);
                task.Name = taskNewName.Text;
                task.Duration += TimeSpan.FromMinutes(int.Parse(durBox.Text));
                AccountManager.SaveChanges();
            }
        }
    }
}

