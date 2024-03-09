using DeepWork.Models;
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
			ViewModel = App.GetService<TaskManagementViewModel>();
			this.InitializeComponent();
			// Todo: Remove TimeDateToStringConverter
		}

		private void LongTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Any())
			{
				ViewModel.SelectLongTask(e.AddedItems.First() as LongTaskViewModel);
				splitView.IsPaneOpen = true;
			}
		}

		private void FinishShortTaskCheckBox_Click(object sender, RoutedEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			checkBox.IsChecked = false;
			ViewModel.FinishShortTask(checkBox.Tag as string);
		}

		private void FinishLongTaskCheckBox_Click(object sender, RoutedEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			checkBox.IsChecked = false;
			ViewModel.FinishLongTask(checkBox.Tag as string);
		}

		private async void AddLongTaskButton_Click(object sender, RoutedEventArgs e)
		{
			AddLongTaskDialogPage content = new();

			ContentDialog dialog = new()
			{
				XamlRoot = this.XamlRoot,
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
				XamlRoot = this.XamlRoot,
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

		private void EditLongTaskButton_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}


		private void EditShortTaskButton_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
