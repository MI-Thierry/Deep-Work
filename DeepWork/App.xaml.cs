using DeepWork.Services;
using DeepWork.ViewModels.Pages;
using DeepWork.Views.Pages;
using DeepWork.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace DeepWork
{
	public partial class App : Application
	{
		private INavigationWindow _window;
		private static IServiceProvider _serviceProvider;
		public App()
		{
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
			services.AddSingleton<NavigationViewViewModel>();
			services.AddSingleton<TaskManagementViewModel>();

			_serviceProvider = services.BuildServiceProvider();

			// Create the MainWindow.
			_window = GetService<INavigationWindow>();
			_window.ActivateWindow();
			_window.Navigate(typeof(NavigationViewPage), null, new EntranceNavigationTransitionInfo());
		}

	}
}
