using DeepWork.Infrastructure.Common;
using DeepWork.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeepWork.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        string connectionString = configurationManager.GetConnectionString("SqliteConnection")
            ?? throw new InvalidOperationException("Failed to get SqliteConnection from configurations");

        services.AddSingleton<IDeepWorkRepositories>(new DeepWorkRepositories(connectionString));

        return services;
    }
}
