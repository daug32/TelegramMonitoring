using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Builders;
using Monitoring.Core.Implementation.Services;
using Monitoring.Core.Implementation.Validators;

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

            // Validators
            services.AddScoped<IValidator<ProjectConfiguration>, ProjectConfigurationValidator>();
            services.AddScoped<IValidator<AppMonitoringConfiguration>, MonitoringConfigurationValidator>();
            services.AddScoped<IValidator<TelegramBotConfiguration>, TelegramBotConfigurationValidator>();

            return services;
        }
    }
}