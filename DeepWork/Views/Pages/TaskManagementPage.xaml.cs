using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace DeepWork.Views.Pages
{
	public sealed partial class TaskManagementPage : Page
	{
		public TaskManagementViewModel ViewModel { get; set; }
		public TaskManagementPage()
		{
			ViewModel = new TaskManagementViewModel();
			this.InitializeComponent();
		}

		private void LongTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Any())
				ViewModel.SelectLongTask(e.AddedItems.First() as LongTaskViewModel);
		}
		private void ShortTaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Any())
				ViewModel.SelectShortTask(e.AddedItems.First() as ShortTaskViewModel);
		}

		private void FinishShortTaskCheckBox_Click(object sender, RoutedEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			checkBox.IsChecked = false;
			ViewModel.FinishShortTask((int)checkBox.Tag);
		}

		private void FinishLongTaskCheckBox_Click(object sender, RoutedEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			checkBox.IsChecked = false;
			ViewModel.FinishLongTask((int)checkBox.Tag);
		}

		private async void AddLongTaskButton_Click(object sender, RoutedEventArgs e)
		{
			AddLongTaskDialogPage content = new();

			ContentDialog dialog = new()
			{
				XamlRoot = XamlRoot,
				Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
				Title = "Add Long Task",
				PrimaryButtonText = "Add",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				Content = content
			};

			ContentDialogResult result = await dialog.ShowAsync();
			if (result is ContentDialogResult.Primary)
				ViewModel.AddLongTask(content.ViewModel);
		}

		private async void AddShortTaskButton_Click(object sender, RoutedEventArgs e)
		{
			AddShortTaskDialogPage content = new();

			ContentDialog dialog = new()
			{
				XamlRoot = XamlRoot,
				Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
				Title = "Add Short Task",
				PrimaryButtonText = "Add",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				Content = content
			};

			ContentDialogResult result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
				ViewModel.AddShortTask(content.ViewModel);
		}

		private async void EditLongTaskButton_Click(object sender, RoutedEventArgs e)
		{
			AddLongTaskDialogPage content = new()
			{
				ViewModel = ViewModel.SelectedLongTask
			};

			ContentDialog dialog = new()
			{
				XamlRoot = XamlRoot,
				Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
				Title = "Add Long Task",
				PrimaryButtonText = "Edit",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				Content = content
			};

			ContentDialogResult result = await dialog.ShowAsync();
			if (result is ContentDialogResult.Primary)
				ViewModel.EditSelectedLongTask(content.ViewModel);
		}

		private async void EditShortTaskButton_Click(object sender, RoutedEventArgs e)
		{
			AddShortTaskDialogPage content = new()
			{
				ViewModel = ViewModel.SelectedShortTask
			};

			ContentDialog dialog = new()
			{
				XamlRoot = XamlRoot,
				Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
				Title = "Add Short Task",
				PrimaryButtonText = "Edit",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				Content = content
			};

			ContentDialogResult result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
				ViewModel.EditShortTask(content.ViewModel);
		}
		private async void DeleteLongTaskMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
		{
			ContentDialog dialog = new()
			{
				XamlRoot = this.XamlRoot,
				Title = "Warning",
				PrimaryButtonText = "Yes",
				CloseButtonText = "No",
				DefaultButton = ContentDialogButton.Close,
				Content = "Are you sure to you want to delete this long task"
			};
			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
				ViewModel.DeleteLongTask(ViewModel.SelectedLongTask.Id);
		}

		private void OpenShortTasksMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
		{
			splitView.IsPaneOpen = true;
		}

		private async void DeleteShortTaskButton_Click(object sender, RoutedEventArgs e)
		{
			ContentDialog dialog = new()
			{
				XamlRoot = this.XamlRoot,
				Title = "Warning",
				PrimaryButtonText = "Yes",
				CloseButtonText = "No",
				DefaultButton = ContentDialogButton.Close,
				Content = "Are you sure to you want to delete this short task"
			};
			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
				ViewModel.DeleteShortTask(ViewModel.SelectedShortTask.Id);
		}
	}
}
