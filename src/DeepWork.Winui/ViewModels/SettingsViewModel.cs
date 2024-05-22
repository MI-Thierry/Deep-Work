using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Reflection;

namespace DeepWork.Winui.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
	[ObservableProperty]
	private string _appVersion = string.Empty;

	[ObservableProperty]
	private string _copyrightInformation = string.Empty;

	[ObservableProperty]
	private ElementTheme _currentTheme;

	public SettingsViewModel()
	{
		_currentTheme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
		_appVersion = Assembly.GetExecutingAssembly()!.GetName()!.Version!.ToString();
		_copyrightInformation = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).LegalCopyright!;
	}
}
