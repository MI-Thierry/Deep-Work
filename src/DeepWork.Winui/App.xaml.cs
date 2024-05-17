using DeepWork.UseCases.LongTasks.Create;
using DeepWork.Domain.Entities;
using DeepWork.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Collections.Generic;

namespace DeepWork.Winui;

public partial class App : Application
{
    public IHost? AppHost { get; private set; }
    public App()
    {
        this.InitializeComponent();
    }

    public TResult? GetService<TResult>()
    {
        ArgumentNullException.ThrowIfNull(AppHost);
        return AppHost.Services.GetService<TResult>();
    }

    public TResult GetRequiredService<TResult>() where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(AppHost);
        return AppHost.Services.GetRequiredService<TResult>();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = Host.CreateApplicationBuilder();

        string contentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeepWork");
        Directory.CreateDirectory(contentPath);

        builder.Environment.ContentRootPath = contentPath;
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddJsonFile("appsettings.json", false);
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:SQLiteConnection"] = Path.Combine(builder.Environment.ContentRootPath, "DeepWork.db")
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddHostedService<ApplicationHost>();

        // Add Infrastructure to windows-platform app
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(LongTask).Assembly, typeof(CreateLongTaskCommand).Assembly);
        });

        AppHost = builder.Build();

        await AppHost.RunAsync();
    }
}