using DeepWork.Services;
using DeepWork.ViewModels.Pages;
using DeepWork.ViewModels.Windows;
using DeepWork.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace DeepWork
{
	public partial class App : Application
	{
		private static IServiceProvider _serviceProvider;
		public Window Window { get; private set; }
		public App()
		{
			// Todo: Get theme from AccountManagementService.
			this.InitializeComponent();
		}

		/// <summary>
		/// It gets the services from service provider
		/// </summary>
		/// <typeparam name="T">Type of service you want to get</typeparam>
		/// <returns>Requested service</returns>
		public static T GetService<T>()
		{
			return _serviceProvider.GetService<T>();
		}

		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
			// Initializing application's service provider.
			IServiceCollection services = new ServiceCollection();
			services.AddSingleton<AccountManagementService>();
			services.AddSingleton<NavigationWindow>();

			// Adding view models for the views
			services.AddSingleton<TaskManagementViewModel>();
			services.AddSingleton<NavigationWindowViewModel>();

			// Building service provider.
			_serviceProvider = services.BuildServiceProvider();

			// Getting account management services and creating windows.
			AccountManagementService accountManager = GetService<AccountManagementService>();
			if (accountManager.IsAccountAvailable)
				Window = GetService<NavigationWindow>();
			else
				Window = new SignupWindow();
			Window.Activate();
		}
	}
}
