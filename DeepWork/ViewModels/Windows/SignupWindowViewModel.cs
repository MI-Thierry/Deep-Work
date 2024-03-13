using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.Models;
using DeepWork.Services;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Threading.Tasks;

namespace DeepWork.ViewModels.Windows;

public partial class SignupWindowViewModel : ObservableObject
{
	private readonly AccountManagementService _accountManager;

	[ObservableProperty]
	private string _firstname;

	[ObservableProperty]
	private string _lastname;

	[ObservableProperty]
	private string _password;

	public SignupWindowViewModel()
	{
		_accountManager = App.GetService<AccountManagementService>();
	}

	[RelayCommand]
	private void Signup()
	{
		Account account = _accountManager.CreateAccount($"{Firstname} {Lastname}", Password);
		_accountManager.ActivateAccount(account);
		AppInstance.Restart("Restarting");
	}
}
