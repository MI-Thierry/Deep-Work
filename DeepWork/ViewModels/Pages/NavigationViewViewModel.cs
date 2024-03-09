using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Views.Pages;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace DeepWork.ViewModels.Pages;

public partial class NavigationViewViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<NavigationViewItem> _menuItems = new()
	{
		new NavigationViewItem
		{
			Content = "Task Manager",
			// Todo: Change this icon.
			Icon = new FontIcon() { Glyph = "\uE9D5" },
			Tag = typeof(TaskManagementPage)
		}
	};
}
