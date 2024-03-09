using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeepWork.Views.Pages;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        this.InitializeComponent();
    }

	private void ChangeThemeRadioButtonChecked(object sender, RoutedEventArgs e)
	{
		// Grid is the xamlroot of this window.
		if (sender is RadioButton { Tag: string selectedTheme })
			((sender as RadioButton).XamlRoot.Content as Grid).RequestedTheme = Enum.Parse<ElementTheme>(selectedTheme);
	}
}
