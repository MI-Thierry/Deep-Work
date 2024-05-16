using DeepWork.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeepWork.SharedKernel;
using DeepWork.Domain.Entities;

namespace DeepWork.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        string connectionString = configurationManager.GetConnectionString("SqliteConnection")
            ?? throw new InvalidOperationException("Failed to get SqliteConnection from configurations");

        services.AddSingleton<IRepository<LongTask>>(new LongTasksRepository(connectionString));
        services.AddSingleton<IRepository<ShortTask>>(new ShortTasksRepository(connectionString));

        return services;
    }
}
