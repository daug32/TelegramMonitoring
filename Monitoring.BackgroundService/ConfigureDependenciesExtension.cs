using Monitoring.Core;

namespace Monitoring.BackgroundService;

public static class ConfigureDependenciesExtension
{
    public static IServiceCollection ConfigureDependencies(
        this IServiceCollection services )
    {
        services.AddMonitoringDependencies();        
        
        services.AddSingleton<HttpClient, HttpClient>();

        return services;
    }
}