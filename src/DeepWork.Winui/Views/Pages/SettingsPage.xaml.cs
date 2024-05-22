using DeepWork.Winui.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeepWork.Winui.Views.Pages;
public sealed partial class SettingsPage : Page
{
	public SettingsPage()
	{
		this.InitializeComponent();
	}

	private void ChangeThemeRadioButton_Checked(object sender, RoutedEventArgs e)
	{
		// Grid is the XamlRoot of this window.
		if (sender is RadioButton { Tag: string selectedTheme })
		{
			ElementTheme theme = Enum.Parse<ElementTheme>(selectedTheme);
			if (theme != ElementTheme.Default)
				((sender as RadioButton)!.XamlRoot.Content as FrameworkElement)!.RequestedTheme = theme;
			else
			{
				((sender as RadioButton)!.XamlRoot.Content as FrameworkElement)!.RequestedTheme =
					Application.Current.RequestedTheme == ApplicationTheme.Light
					? ElementTheme.Light : ElementTheme.Dark;
			}

			if (DataContext is SettingsViewModel viewModel)
				viewModel.CurrentTheme = theme;
		}
	}
}
