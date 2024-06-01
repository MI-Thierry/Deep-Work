using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.UI.Shared;
using System.Collections.ObjectModel;

namespace DeepWork.Maui.ViewModels;

public partial class SettingViewModel : ObservableObject
{
	[ObservableProperty]
	public string _name = string.Empty;

	[ObservableProperty]
	public string? _description = string.Empty;
}

public partial class SettingsViewModel : ObservableObject
{
	private readonly IAppPreferences _appPreferences;
	public ObservableCollection<SettingViewModel> Settings { get; set; } = [];

	public SettingsViewModel()
	{
		_appPreferences = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IAppPreferences>();
		PopulateSettings();
	}

	public void PopulateSettings()
	{
		string? theme = _appPreferences.Get<string>("Theme");
		theme = theme == null || theme == "Unspecified" ? "System default" : theme + " Theme";
		Settings.Add(new SettingViewModel { Name = "Theme", Description = theme });
	}
}
