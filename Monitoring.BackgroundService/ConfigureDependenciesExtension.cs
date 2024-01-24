using Monitoring.Core;

namespace Monitoring.BackgroundService
{
    public static class ConfigureDependenciesExtension
    {
        public static IServiceCollection ConfigureDependencies(
            this IServiceCollection services )
        {
            services.AddMonitoring();
            services.AddSingleton<HttpClient>();

            return services;
        }
    }
}