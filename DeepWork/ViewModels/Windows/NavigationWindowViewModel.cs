using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Views.Pages;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace DeepWork.ViewModels.Windows;

public partial class NavigationWindowViewModel : ObservableObject
{
	[ObservableProperty]
	private double _navViewCompactModeThresholdWidth;

	[ObservableProperty]
	private ObservableCollection<NavigationViewItem> _menuItems =
	[
		new NavigationViewItem
		{
			Content = "Task Manager",
			Icon = new FontIcon() { Glyph = "\uE9D5" },
			Tag = typeof(TaskManagementPage)
		}
	];
}
