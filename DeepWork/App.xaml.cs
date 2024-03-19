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
		public Window Window { get; private set; }
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
			services.AddSingleton<NavigationWindow>();

			// Adding view models for the views
			services.AddSingleton<NavigationWindowViewModel>();
			services.AddSingleton((serviceProvider) =>
			{
				return new PomodoroTimerViewModel(
					serviceProvider.GetRequiredService<AccountManagementService>(),
					Window.DispatcherQueue
					);
			});

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

		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
			if (_accountManager.IsAccountAvailable)
				Window = GetService<NavigationWindow>();
			else
				Window = new SignupWindow();
			Window.Activate();

			// To ensure all Notification handling happens in this process instance, register for
			// NotificationInvoked before calling Register(). Without this a new process will
			// be launched to handle the notification.
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
			DispatcherQueue dispatcherQueue = Window?.DispatcherQueue ?? DispatcherQueue.GetForCurrentThread();
			PomodoroTimerViewModel pmdrVm = GetService<PomodoroTimerViewModel>();

			dispatcherQueue.TryEnqueue(delegate
			{
				switch (args.Arguments["action"])
				{
					case "stopPomodoroSession":
						pmdrVm.StopPomodoroSession();
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
			WindowHelper.ShowWindow(Window);
		}

		private void NotificationManager_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
		{
			HandleNotification(args);
		}
	}
}
