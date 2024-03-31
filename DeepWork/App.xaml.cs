using DeepWork.Data;
using DeepWork.Helpers;
using DeepWork.Services;
using DeepWork.ViewModels.Pages;
using DeepWork.ViewModels.Windows;
using DeepWork.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using System;
using System.IO;

namespace DeepWork
{
	public partial class App : Application
	{
		private readonly AccountManagementService _accountManager;
		private static IServiceProvider _serviceProvider;
		public Window MainWindow { get; private set; }
		public string AppDataPath { get; private set; }
		public string DbPath { get; private set; }

		public App()
		{
			this.InitializeComponent();

			// Create a path to appdata directory
			string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			AppDataPath = Path.Join(localAppData, "Deep Work");
			DbPath = Path.Join(AppDataPath, "DeepWorkData.db");

			// Checking appdata directory availability
			if (!Directory.Exists(AppDataPath))
				Directory.CreateDirectory(AppDataPath);

			// Initializing application's service provider.
			IServiceCollection services = new ServiceCollection();
			services.AddSqlite<AccountContext>($"Data Source={DbPath}");
			services.AddSingleton<AccountManagementService>();
			services.AddTransient<NavigationWindow>();
			services.AddTransient<SignupWindow>();

			// Adding view models for the views
			services.AddSingleton<NavigationWindowViewModel>();
			services.AddSingleton<PomodoroTimerViewModel>();

			// Building service provider.
			_serviceProvider = services.BuildServiceProvider();

			// Getting account management services and creating windows.
			_accountManager = GetService<AccountManagementService>();

			if (_accountManager.IsAccountAvailable)
				Current.RequestedTheme = _accountManager.ActiveAccount.Theme == ElementTheme.Light ? ApplicationTheme.Light
										 : _accountManager.ActiveAccount.Theme == ElementTheme.Dark ? ApplicationTheme.Dark
										 : Current.RequestedTheme;
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

		public static object GetService(Type serviceType)
		{
			return _serviceProvider.GetService(serviceType);
		}

		public void NavigateWindow(Type windowType)
		{
			PomodoroTimerViewModel viewModel = _serviceProvider.GetRequiredService<PomodoroTimerViewModel>();
			viewModel.StopPomodoroSession();

			Window win = _serviceProvider.GetRequiredService(windowType) as Window;
			win.Activate();

			MainWindow?.Close();
			MainWindow = win;
		}

		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
			if (_accountManager.IsAccountAvailable)
				NavigateWindow(typeof(NavigationWindow));
			else
				NavigateWindow(typeof(SignupWindow));

			AppNotificationManager notificationManager = AppNotificationManager.Default;
			notificationManager.NotificationInvoked += NotificationManager_NotificationInvoked;
			notificationManager.Register();

			AppActivationArguments activatedArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
			var activationKind = activatedArgs.Kind;
			if (activationKind != ExtendedActivationKind.AppNotification)
				LaunchAndBringToForegroundIfNeeded();
			else
				HandleNotification((AppNotificationActivatedEventArgs)activatedArgs.Data);
		}

		private void HandleNotification(AppNotificationActivatedEventArgs args)
		{
			DispatcherQueue dispatcherQueue = MainWindow?.DispatcherQueue ?? DispatcherQueue.GetForCurrentThread();
			PomodoroTimerViewModel pomViewModel = GetService<PomodoroTimerViewModel>();

			dispatcherQueue.TryEnqueue(delegate
			{
				switch (args.Arguments["action"])
				{
					case "stopPomodoroSession":
						pomViewModel.StopPomodoroSession();
						LaunchAndBringToForegroundIfNeeded();
						break;

					case "dismiss":
						break;

					default:
						LaunchAndBringToForegroundIfNeeded();
						break;
				}
			});
		}

		private void LaunchAndBringToForegroundIfNeeded()
		{
			WindowHelper.ShowWindow(MainWindow);
		}

		private void NotificationManager_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
		{
			HandleNotification(args);
		}
	}
}
