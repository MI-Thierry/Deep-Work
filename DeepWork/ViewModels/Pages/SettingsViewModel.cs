using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.Models;
using DeepWork.Services;
using DeepWork.Views.Windows;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using WinRT;

namespace DeepWork.ViewModels.Pages
{
	public partial class SettingsViewModel : ObservableObject
	{
		private readonly AccountManagementService _accountManager;

		[ObservableProperty]
		private ObservableCollection<AccountViewModel> _accountsList;

		[ObservableProperty]
		private AccountViewModel _openAccount;

		[ObservableProperty]
		private string _appVersion = string.Empty;

		[ObservableProperty]
		private string _copyrightInformation = string.Empty;

		private ElementTheme _currentTheme;
		public ElementTheme CurrentTheme
		{
			get => _currentTheme;
			set {
				SetProperty(ref _currentTheme, value);
				_accountManager.ChangeTheme(value);
			}
		}

		public SettingsViewModel()
		{
			_accountManager = App.GetService<AccountManagementService>();
			_openAccount = _accountManager.ActiveAccount;
			_accountsList = [];
			foreach (var account in _accountManager.AvailableAccounts)
				_accountsList.Add(account);

			_currentTheme = _accountManager.ActiveAccount.Theme;
			_appVersion = Assembly.GetExecutingAssembly()?.GetName().Version.ToString();
			_copyrightInformation = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
		}

		public bool SignInAccount(int accountId, string password)
		{
			Account account = _accountManager.SignInAccount(accountId, password);
			if (account == null)
				return false;

			OpenAccount = account;
			return true;
		}

		public AccountViewModel CreateAccount(string username, string password)
		{
			AccountViewModel account = _accountManager.CreateAccount(username, password);
			AccountsList.Add(account);
			return account;
		}

		[RelayCommand]
		private void SignOutAccount()
		{
			OpenAccount = null;
			_accountManager.SignOutAccount();
			Application.Current.As<App>().NavigateWindow(typeof(SignupWindow));
		}
	}
}
