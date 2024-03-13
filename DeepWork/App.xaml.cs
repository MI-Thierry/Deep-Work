using DeepWork.Data;
using DeepWork.Services;
using DeepWork.ViewModels.Pages;
using DeepWork.ViewModels.Windows;
using DeepWork.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.IO;

namespace DeepWork
{
	public partial class App : Application
	{
		private static IServiceProvider _serviceProvider;
		public Window Window { get; private set; }
		public string AppDataPath { get; private set; }
		public string DbPath { get; private set; }
		public App()
		{
			// Todo: Get theme from AccountManagementService.
			this.InitializeComponent();

			// Create a path to appdata directory
			string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			AppDataPath = Path.Join(localAppData, "Deep Work");
			DbPath = Path.Join(AppDataPath, "DeepWorkData.db");

			// Checking appdata directory availability
			if (!Directory.Exists(AppDataPath))
				Directory.CreateDirectory(AppDataPath);
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
			services.AddSqlite<AccountContext>($"Data Source={DbPath}");
			services.AddSingleton<AccountManagementService>();
			services.AddSingleton<NavigationWindow>();

			// Adding view models for the views
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
