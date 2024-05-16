﻿using DeepWork.UseCases.LongTasks.Create;
using DeepWork.Domain.Entities;
using DeepWork.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Reflection;

namespace DeepWork.Winui;

public partial class App : Application
{
    public IHost AppHost { get; private set; }
    public App()
    {
        this.InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();
        builder.Configuration.AddJsonFile("appsettings.json", false);

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddHostedService<ApplicationHost>();
        builder.Services.AddTransient<MainWindow>();

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