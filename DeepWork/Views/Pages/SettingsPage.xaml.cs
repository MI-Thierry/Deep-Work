using DeepWork.Helpers;
using DeepWork.ViewModels.Pages;
using DeepWork.Views.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using WinRT;

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
		// Grid is the XamlRoot of this window.
		if (sender is RadioButton { Tag: string selectedTheme })
		{
			ElementTheme theme = Enum.Parse<ElementTheme>(selectedTheme);
			((sender as RadioButton).XamlRoot.Content as Grid).RequestedTheme = theme;
			ViewModel.CurrentTheme = theme;
		}
	}

	private void ManageAccountHyperlinkButton_Click(object sender, RoutedEventArgs e)
	{
		throw new NotImplementedException();
	}

	private async void SignInButton_Click(object sender, RoutedEventArgs e)
	{
		int id = (int)(sender as Button).Tag;

		SignInDialogPage content = new()
		{
			Username = ViewModel.AccountsList.First(account => account.Id == id).UserName
		};

		ContentDialog contentDialog = new()
		{
			XamlRoot = XamlRoot,
			Title = "Sign In",
			PrimaryButtonText = "Sign In",
			CloseButtonText = "Cancel",
			DefaultButton = ContentDialogButton.Primary,
			Content = content
		};

		ContentDialogResult result = await contentDialog.ShowAsync();

		if (result == ContentDialogResult.Primary)
			if (!ViewModel.SignInAccount(id, content.Password))
				await WindowHelper.WarningDialog($"Failed to Sign In {content.Username}'s account", XamlRoot);
	}

	private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
	{
		Application.Current.As<App>().NavigateWindow(typeof(SignupWindow));
	}
}
