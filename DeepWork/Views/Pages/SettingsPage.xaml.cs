using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeepWork.Views.Pages;

public sealed partial class SettingsPage : Page
{
	public SettingsViewModel ViewModel { get; set; }
    public SettingsPage()
    {
        this.InitializeComponent();
		ViewModel = DataContext as SettingsViewModel;
    }

	private void ChangeThemeRadioButtonChecked(object sender, RoutedEventArgs e)
	{
		// Grid is the xamlroot of this window.
		if (sender is RadioButton { Tag: string selectedTheme })
		{
			ElementTheme theme = Enum.Parse<ElementTheme>(selectedTheme);
			((sender as RadioButton).XamlRoot.Content as Grid).RequestedTheme = theme;
			ViewModel.CurrentTheme = theme;
		}
	}
}
