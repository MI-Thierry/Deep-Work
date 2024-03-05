using DeepWork.MVVM.Models;
using DeepWork.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace DeepWork.MVVM.Views
{
    public sealed partial class HistoryPage : Page
    {
        private List<TaskHistory> TasksHistoryTree { get; set; }
        public HistoryPage()
        {
            TasksHistoryTree = new List<TaskHistory>();
            this.InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            AccountManagementServices accountManager = App.ServiceProvider
                .GetRequiredService<AccountManagementServices>();

            if (accountManager.IsAccountAvailable)
                TasksHistoryTree = accountManager.GetAccountHistory();
        }
    }

    public class TreeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LongTaskTemplate { get; set; }
        public DataTemplate ShortTaskTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            TaskHistory tasksHistoryItem = item as TaskHistory;
            if (tasksHistoryItem.Type == TaskType.LongTask)
                return LongTaskTemplate;
            return ShortTaskTemplate;
        }
    }
}
