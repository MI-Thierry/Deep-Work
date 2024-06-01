namespace DeepWork.Maui;

public partial class App : Application
{
	public static AppTheme StartupAppTheme { get; set; } = AppTheme.Unspecified;
    public App()
    {
        InitializeComponent();
		Current!.UserAppTheme = StartupAppTheme;
		MainPage = new AppShell();
    }
}
