using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeepWork.Services;
using Microsoft.Windows.AppLifecycle;

namespace DeepWork.ViewModels.Pages;

public partial class SignupViewModel : ObservableObject
{
	private readonly AccountManagementService _accountManager;

	[ObservableProperty]
	private string _firstname;

	[ObservableProperty]
	private string _lastname;

	[ObservableProperty]
	private string _password;

	public SignupViewModel()
	{
		_accountManager = App.GetService<AccountManagementService>();
	}

	[RelayCommand]
	private void Signup()
	{
		_accountManager.CreateAccount($"{Firstname} {Lastname}", Password);
		AppInstance.Restart("Restarting");
	}
}
