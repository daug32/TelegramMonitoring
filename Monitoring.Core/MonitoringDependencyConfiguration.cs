using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Implementation.Builders;
using Monitoring.Core.Implementation.Services;

namespace Monitoring.Core
{
    public static class MonitoringDependencyConfiguration
    {
        public static IServiceCollection AddMonitoring( this IServiceCollection services )
        {
            // Services
            services.AddScoped<IProjectNotificator, ProjectNotificator>();
            services.AddScoped<IProjectNotificatorScheduler, ProjectNotificatorScheduler>();

            // Builders
            services.AddScoped<IProjectMonitoringBuilder, ProjectMonitoringBuilder>();
            services.AddScoped<ITelegramHandlerBuilder, TelegramHandlerBuilder>();

            return services;
        }
    }
}