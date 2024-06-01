using DeepWork.UI.Shared;

namespace DeepWork.UnitTests.UserInterface;
public class PreferencesCRUD
{
	private readonly Stream _stream = new MemoryStream();
	private IAppPreferences _preferences;

	public PreferencesCRUD()
	{
		_preferences = new AppPreferences(_stream);
	}

	[Fact]
	void PreferenceCreateAndRead()
	{
		string strKey = "Key";
		string strValue = "Test Value";
		_preferences.Set(strKey, strValue);
		Assert.Equal(strValue, _preferences.Get(strKey, default(string)));
		Assert.Throws<InvalidOperationException>(() => _preferences.Set("Guid", Guid.NewGuid()));
		Assert.Throws<InvalidOperationException>(() => _preferences.Get("Guid", default(Guid)));
	}

	[Fact]
	void PreferenceUpdate()
	{
		string key = "Key";
		string value = "Test Value";
		string updatedValue = "Updated Test Value";
		_preferences.Set(key, value);
		_preferences.Set(key, updatedValue);
		Assert.Equal(updatedValue, _preferences.Get(key, default(string)));
	}

	[Fact]
	void PreferenceDelete()
	{
		string key = "Key";
		string value = "Test Value";
		_preferences.Set(key, value);
		_preferences.Remove(key);
		Assert.False(_preferences.ContainsKey(key));
	}
}
