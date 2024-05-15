using DeepWork.Infrastructure.Common;
using DeepWork.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeepWork.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection servcies, IConfigurationManager configurationManager)
    {
        string connectionString = configurationManager.GetConnectionString("SqliteConnection")
            ?? throw new InvalidOperationException("Failed to get SqliteConnection from configurations");

        servcies.AddSingleton<IRepository>(new DeepWorkRepository(connectionString));

        return servcies;
    }
}
