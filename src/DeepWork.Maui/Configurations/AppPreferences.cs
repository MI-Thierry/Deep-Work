using DeepWork.UI.Shared;

namespace DeepWork.Maui.Configurations;
public class AppPreferences : IAppPreferences
{
	private readonly IPreferences _preferences = Preferences.Default;
	public void Clear(string? sharedName = null)
	{
		_preferences.Clear(sharedName);
	}

	public bool ContainsKey(string key, string? sharedName = null)
	{
		return _preferences.ContainsKey(key, sharedName);
	}

	public T? Get<T>(string key, string? sharedName = null) where T : notnull
	{
		return _preferences.Get<T>(key, default!, sharedName);
	}

	public void Remove(string key, string? sharedName = null)
	{
		_preferences.Remove(key, sharedName);
	}

	public void Set<T>(string key, T value, string? sharedName = null) where T : notnull
	{
		_preferences.Set(key, value, sharedName);
	}
}
