using DeepWork.UseCases.LongTasks.Create;
using DeepWork.Domain.Entities;
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
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(LongTask).Assembly, typeof(CreateLongTaskCommand).Assembly);
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}