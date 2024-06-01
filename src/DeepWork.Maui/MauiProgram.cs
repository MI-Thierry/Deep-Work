using DeepWork.UseCases.LongTasks.Create;
using DeepWork.Domain.Entities;
using DeepWork.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using DeepWork.UI.Shared;
using DeepWork.Maui.Configurations;

namespace DeepWork.Maui;

public static class MauiProgram
{
	public static string ContentRootPath { get; private set; } = string.Empty;
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesomeBrands.otf", "FontBrands");
				fonts.AddFont("FontAwesomeRegular.otf", "FontRegular");
				fonts.AddFont("FontAwesomeSolid.otf", "FontSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		ContentRootPath = Path.Combine(FileSystem.AppDataDirectory, "DeepWork");
		Directory.CreateDirectory(ContentRootPath);

		// Add configurations
		using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
		builder.Configuration.AddJsonStream(stream);
		builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
		{
			["ConnectionStrings:SQLiteConnection"] = Path.Combine(ContentRootPath, "DeepWork.db")
		});

		// Add services
		builder.Services.AddSingleton<IAppPreferences, AppPreferences>();
		builder.Services.AddInfrastructureServices(builder.Configuration);
		builder.Services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblies(typeof(LongTask).Assembly, typeof(CreateLongTaskCommand).Assembly);
		});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		MauiApp mauiApp = builder.Build();

		// Setting the app theme before App constructor run
		IAppPreferences preferences = mauiApp.Services.GetRequiredService<IAppPreferences>();
		string? theme = preferences.Get<string>("Theme");
		if (theme != null)
		{
			App.StartupAppTheme = Enum.Parse<AppTheme>(theme);
		}

		return mauiApp;
	}
}