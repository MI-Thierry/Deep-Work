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
			Content = "Home",
			Icon = new FontIcon(){Glyph="\uea8a"},
			Tag = typeof(HomePage)
		},
	};
}
