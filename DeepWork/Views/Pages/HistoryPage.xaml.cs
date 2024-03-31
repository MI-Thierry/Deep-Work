using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.Views.Pages
{
	public sealed partial class HistoryPage : Page
	{
		public HistoryPage()
		{
			this.InitializeComponent();
		}
	}

	public class TreeTemplateSelector : DataTemplateSelector
	{
		public DataTemplate LongTaskTemplate { get; set; }
		public DataTemplate ShortTaskTemplate { get; set; }
		protected override DataTemplate SelectTemplateCore(object item)
		{
			TaskViewModel tasksHistoryItem = item as TaskViewModel;
			return tasksHistoryItem.Type == TaskType.LongTask ? LongTaskTemplate : ShortTaskTemplate;
		}
	}
}
