using CommunityToolkit.Mvvm.ComponentModel;using DeepWork.Services;using Microsoft.UI.Xaml;using System.Diagnostics;using System.Reflection;namespace DeepWork.ViewModels.Pages{	public partial class SettingsViewModel : ObservableObject	{		private readonly AccountManagementService _accountManager;		[ObservableProperty]		private string _appVersion = string.Empty;		[ObservableProperty]		private string _copyrightInformation = string.Empty;		private ElementTheme _currentTheme;
		public ElementTheme CurrentTheme
		{
			get => _currentTheme;
			set {
				SetProperty(ref _currentTheme, value);
				_accountManager.ChangeTheme(value);
			}
		}

		public SettingsViewModel()		{			_accountManager = App.GetService<AccountManagementService>();
			_currentTheme = _accountManager.ActiveAccount.Theme;			_appVersion = Assembly.GetExecutingAssembly()?.GetName().Version.ToString();			_copyrightInformation = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;		}	}}