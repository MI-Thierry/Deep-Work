using DeepWork.Services;
using DeepWork.ViewModels.Pages;
using DeepWork.Views.Pages;
using DeepWork.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace DeepWork
{
	public partial class App : Application
	{
		private static IServiceProvider _serviceProvider;
		public INavigationWindow Window { get; private set; }
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
			services.AddTransient<INavigationWindow, MainWindow>();

			// Adding view models for the views
			services.AddSingleton<NavigationManagementViewModel>();
			services.AddSingleton<TaskManagementViewModel>();

			_serviceProvider = services.BuildServiceProvider();

			// Create the MainWindow.
			Window = GetService<INavigationWindow>();
			Window.ActivateWindow();

			AccountManagementService accountManager = GetService<AccountManagementService>();
			if (accountManager.IsAccountAvailable)
				Window.Navigate(typeof(NavigationManagementPage), null, new EntranceNavigationTransitionInfo());
			else
				Window.Navigate(typeof(SignupPage), null, new EntranceNavigationTransitionInfo());
		}
	}
}
