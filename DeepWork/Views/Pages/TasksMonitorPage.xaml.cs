using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.Views.Pages
{
	public sealed partial class TasksMonitorPage : Page
	{
		public TasksMonitorViewModel ViewModel { get; set; }
		public TasksMonitorPage()
		{
			this.InitializeComponent();
			ViewModel = new TasksMonitorViewModel(ActualTheme);
		}

		private Visibility IntToVisibilityConverter(int value) =>
			value != 0 ? Visibility.Visible : Visibility.Collapsed;

		private bool VisibilityToBoolConverter(Visibility value) =>
			value != Visibility.Visible;

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != 0)
				ViewModel.PlotTask(e.AddedItems[0] as LongTaskViewModel);
		}

		private void Pivot_ActualThemeChanged(FrameworkElement sender, object args)
		{
			ViewModel.ChangeTheme(sender.ActualTheme);
		}
	}
}
