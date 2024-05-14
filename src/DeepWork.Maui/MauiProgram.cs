using DeepWork.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeepWork.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
        builder.Configuration.AddJsonStream(stream);

        // Add Infrastructure to cross-platform app
        builder.Services.AddInfrastructureServices(builder.Configuration);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}