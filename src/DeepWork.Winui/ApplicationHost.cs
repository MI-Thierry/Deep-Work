using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System.Threading;
using System.Threading.Tasks;

namespace DeepWork.Winui;
public class ApplicationHost(MainWindow window, ILogger<ApplicationHost> logger) : IHostedService
{
    private readonly Window _window = window;
    private readonly ILogger<ApplicationHost> _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The application is starting...");
        if (!cancellationToken.IsCancellationRequested)
            _window.Activate();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The application is stopping...");
        if (!cancellationToken.IsCancellationRequested)
            _window.Close();
        return Task.CompletedTask;
    }
}
