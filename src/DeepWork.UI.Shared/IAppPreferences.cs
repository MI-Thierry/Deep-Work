namespace DeepWork.UI.Shared;
public interface IAppPreferences
{
	bool ContainsKey(string key, string? sharedName = null);

	void Remove(string key, string? sharedName = null);

	void Clear(string? sharedName = null);

	void Set<T>(string key, T value, string? sharedName = null) where T : notnull;

	T? Get<T>(string key, string? sharedName = null) where T : notnull;
}
