using DeepWork.UI.Shared;

namespace DeepWork.Maui.Views;

public class ThemeSettingsPage : ContentPage
{
	public IAppPreferences? _appPreferences;
	public ThemeSettingsPage()
	{
		_appPreferences = null;
		AppTheme currentTheme = Application.Current!.UserAppTheme;
		Title = "Theme";

		var lightThemeButton = new RadioButton()
		{
			Content = "Light",
			GroupName = "ThemeButtons",
			IsChecked = currentTheme == AppTheme.Light
		};
		lightThemeButton.CheckedChanged += (s, e) => SetTheme(AppTheme.Light);

		var darkThemeButton = new RadioButton()
		{
			Content = "Dark",
			GroupName = "ThemeButtons",
			IsChecked = currentTheme == AppTheme.Dark
		};
		darkThemeButton.CheckedChanged += (s, e) => SetTheme(AppTheme.Dark);

		var systemThemeButton = new RadioButton()
		{
			Content = "System default",
			GroupName = "ThemeButtons",
			IsChecked = currentTheme == AppTheme.Unspecified
		};
		systemThemeButton.CheckedChanged += (s, e) => SetTheme(AppTheme.Unspecified);

		Content = new VerticalStackLayout
		{
			Children = {
				lightThemeButton,
				darkThemeButton,
				systemThemeButton
			}
		};
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		_appPreferences = Handler!.MauiContext!.Services.GetRequiredService<IAppPreferences>();
	}

	private void SetTheme(AppTheme theme)
	{
		_appPreferences?.Set("Theme", theme.ToString());
		Application.Current!.UserAppTheme = theme;
	}
}