using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeepWork.Winui;
public class ApplicationHost(ILogger<ApplicationHost> logger) : IHostedService
{
    private static readonly List<Window> _appWindows = [];
    private readonly Window _appRootWindow = CreateWindow<MainWindow>();
    private readonly ILogger<ApplicationHost> _logger = logger;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The application is starting...");

        _appRootWindow.Activate();
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
}
