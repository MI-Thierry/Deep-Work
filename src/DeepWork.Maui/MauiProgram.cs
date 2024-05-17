using DeepWork.UseCases.LongTasks.Create;
using DeepWork.Domain.Entities;
using DeepWork.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeepWork.Maui;

public static class MauiProgram
{
    public static string ContentRootPath { get; private set; } = string.Empty;
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

        ContentRootPath = Path.Combine(FileSystem.AppDataDirectory, "DeepWork");
        Directory.CreateDirectory(ContentRootPath);

        using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
        builder.Configuration.AddJsonStream(stream);
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:SQLiteConnection"] = Path.Combine(ContentRootPath, "DeepWork.db")
        });

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