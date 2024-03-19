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
	private ObservableCollection<NavigationViewItemBase> _menuItems =
	[
		new NavigationViewItem
		{
			Content = "Task Manager",
			Icon = new FontIcon() { Glyph = "\uE9D5" },
			Tag = typeof(TaskManagementPage)
		},
		new NavigationViewItemSeparator(),
		new NavigationViewItem
		{
			Content = "Pomodoro Timer",
			Icon = new FontIcon() { Glyph = "\uEC4A"},
			Tag = typeof(PomodoroTimerPage)
		}
	];
}
