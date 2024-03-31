using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace DeepWork.ViewModels.Pages
{
    public enum TaskType
    {
        None,
        LongTask,
        ShortTask
    };

    public partial class TaskViewModel : ObservableObject
    {
        public TaskType Type { get; set; }
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private DateTimeOffset _finishData;

        [ObservableProperty]
        private ObservableCollection<TaskViewModel> _children = [];
		public string ConvertDateTimeToString(DateTimeOffset date) => date.ToString("ddd, MMM dd, yyyy");
	}
}
