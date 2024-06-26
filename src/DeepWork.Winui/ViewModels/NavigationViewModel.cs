﻿using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Winui.Views.Pages;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace DeepWork.Winui.ViewModels;
public partial class NavigationViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<NavigationViewItemBase> _menuItems =
	[
		new NavigationViewItem
		{
			Content = "Task Manager",
			Icon = new FontIcon() { Glyph = "\uE9D5" },
			Tag = typeof(TaskManagementPage)
		},
	];
}
