using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using System.Runtime.Versioning;

namespace DeepWork.Maui.Views;

[UnsupportedOSPlatform("maccatalyst")]
public partial class TaskManagementPage : ContentPage
{
	private StatusBarBehavior _statusBarBehavior;

	public TaskManagementPage()
	{

		InitializeComponent();

		_statusBarBehavior = new();
		AppTheme theme = Application.Current!.RequestedTheme;
		SetStatusBarBehavior(theme);
		Application.Current!.RequestedThemeChanged += (sender, e) => SetStatusBarBehavior(e.RequestedTheme);
	}

	private void SetStatusBarBehavior(AppTheme theme)
	{
		_statusBarBehavior.StatusBarStyle = theme == AppTheme.Light
			? StatusBarStyle.DarkContent
			: StatusBarStyle.LightContent;

		_statusBarBehavior.StatusBarColor = theme == AppTheme.Light
			? (Color)Application.Current!.Resources["White"]
			: (Color)Application.Current!.Resources["OffBlack"];
	}
}