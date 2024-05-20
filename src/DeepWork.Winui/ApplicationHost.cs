using DeepWork.Winui.Views.Pages;
using DeepWork.Winui.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeepWork.Winui;
public class ApplicationHost(ILogger<ApplicationHost> logger) : IHostedService
{
	private static readonly List<Window> _appWindows = [];
	private readonly MainWindow _appRootWindow = CreateWindow<MainWindow>();
	private readonly ILogger<ApplicationHost> _logger = logger;

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("The application is starting...");

		_appRootWindow.Activate();
        _appRootWindow.MinHeight = 500;
        _appRootWindow.MinWidth = 500;

		if (_appRootWindow is INavigableWindow window)
			window.NavigateTo(typeof(NavigationPage), new EntranceNavigationTransitionInfo());

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("The application is stopping...");

		_appWindows.ForEach(window => window.Close());
		return Task.CompletedTask;
	}

	public static TResult CreateWindow<TResult>() where TResult : Window, new()
	{
		TResult window = new();
		window.Closed += (object sender, WindowEventArgs args) =>
			_appWindows.Remove((Window)sender);

		_appWindows.Add(window);
		return window;
	}

	public static void TrackWindow(Window window)
	{
		window.Closed += (object sender, WindowEventArgs args) =>
		   _appWindows.Remove((Window)sender);

		_appWindows.Add(window);
	}

	public static Window GetElementsWindow(UIElement element)
	{
		Window? window = _appWindows.FirstOrDefault(w => w.Content.XamlRoot == element.XamlRoot);
		return window ?? throw new InvalidOperationException("The element's window in not tracked by application. Call ApplicationHost.TrackWindow() to track it");
	}
}
