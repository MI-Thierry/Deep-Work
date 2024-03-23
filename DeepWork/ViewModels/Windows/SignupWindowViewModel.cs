using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.Models;
using DeepWork.Services;
using DeepWork.ViewModels.Pages;
using DeepWork.Views.Pages;
using DeepWork.Views.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WinRT;

namespace DeepWork.ViewModels.Windows;

public partial class SignupWindowViewModel : ObservableObject
{
	private readonly AccountManagementService _accountManager;
	public int SelectedAccountId { get; set; }

	[ObservableProperty]
	private string _message = string.Empty;

	[ObservableProperty]
	private ObservableCollection<AccountViewModel> _accountList;

	[ObservableProperty]
	private string _firstname;

	[ObservableProperty]
	private string _lastname;

	[ObservableProperty]
	private string _password;

	public SignupWindowViewModel()
	{
		_accountManager = App.GetService<AccountManagementService>();
		_accountList = [];
		foreach (var account in _accountManager.AvailableAccounts)
			_accountList.Add(account);
	}

	[RelayCommand]
	private void Signup()
	{
		string username = $"{Firstname} {Lastname}";
		if (string.IsNullOrWhiteSpace(username))
			Message = "You need to provide at least one name";
		else if (string.IsNullOrEmpty(Password))
			Message = "You need to provide a strong password";
		else
		{
			Account account = _accountManager.CreateAccount(username, Password);
			_accountManager.SignInAccount(account.Id, Password);
			Application.Current.As<App>().NavigateWindow(typeof(NavigationWindow));
		}
	}

	[RelayCommand]
	private async Task Login(XamlRoot xamlRoot)
	{
		SignInDialogPage content = new()
		{
			Username = AccountList.First(accVm => accVm.Id == SelectedAccountId).UserName,
		};

		ContentDialog contentDialog = new()
		{
			XamlRoot = xamlRoot,
			Title = "Login",
			PrimaryButtonText = "Login",
			CloseButtonText = "Cancel",
			DefaultButton = ContentDialogButton.Primary,
			Content = content
		};

		ContentDialogResult result = await contentDialog.ShowAsync();

		if (result == ContentDialogResult.Primary)
		{
			if (_accountManager.SignInAccount(SelectedAccountId, content.Password) == null)
				Message = "Incorrect password.";
			else
				Application.Current.As<App>().NavigateWindow(typeof(NavigationWindow));
		}
	}
}
