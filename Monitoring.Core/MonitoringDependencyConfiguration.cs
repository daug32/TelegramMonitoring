using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Implementation.Monitorings;
using Monitoring.Core.Implementation.Notificators;
using Monitoring.Core.Implementation.Telegram;

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