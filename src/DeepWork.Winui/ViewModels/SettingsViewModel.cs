using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.UI.Shared;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Reflection;

namespace DeepWork.Winui.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
	private IAppPreferences _appPreferences;

	[ObservableProperty]
	private string _appVersion = string.Empty;

	[ObservableProperty]
	private string _copyrightInformation = string.Empty;

	private ElementTheme _currentTheme;

	public ElementTheme CurrentTheme
	{
		get { return _currentTheme; }
		set {
			SetProperty(ref _currentTheme, value);
			_appPreferences.Set("Theme", _currentTheme.ToString());
		}
	}

	public SettingsViewModel()
	{
		_appPreferences = App.GetRequiredService<IAppPreferences>();
		_appVersion = Assembly.GetExecutingAssembly()!.GetName()!.Version!.ToString();
		_copyrightInformation = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).LegalCopyright!;

		string? theme = _appPreferences.Get<string>("Theme");
		_currentTheme = theme != null ? Enum.Parse<ElementTheme>(theme) : ElementTheme.Default;
	}
}
