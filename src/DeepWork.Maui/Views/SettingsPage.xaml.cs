using DeepWork.Maui.ViewModels;

namespace DeepWork.Maui.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

	private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		AppShell.NavigateTo(typeof(ThemeSettingsPage), true);
		(sender as ListView)!.SelectedItem = null;
	}
}