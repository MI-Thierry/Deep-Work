using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeepWork.Views.Pages
{
	public sealed partial class TasksMonitorPage : Page
	{
		public TasksMonitorViewModel ViewModel { get; set; }
		public TasksMonitorPage()
		{
			ViewModel = new TasksMonitorViewModel();
			this.InitializeComponent();
			ViewModel.SetTheme(ActualTheme);
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != 0)
				ViewModel.SelectTask(e.AddedItems[0] as LongTaskViewModel);
		}

		private void Pivot_ActualThemeChanged(FrameworkElement sender, object args)
		{
			ViewModel.SetTheme(sender.ActualTheme);
		}
	}
}
