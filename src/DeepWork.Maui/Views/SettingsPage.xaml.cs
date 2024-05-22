using CommunityToolkit.Maui.Behaviors;

namespace DeepWork.Maui.Views;

public partial class SettingsPage : ContentPage
{
    private StatusBarBehavior _statusBarBehavior;
	public SettingsPage()
	{
		InitializeComponent();
        _statusBarBehavior = new StatusBarBehavior();

	}
}