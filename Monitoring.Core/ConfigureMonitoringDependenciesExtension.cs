using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Builders;
using Monitoring.Core.Builders.Implementation;
using Monitoring.Core.Configurations;
using Monitoring.Core.Services;
using Monitoring.Core.Services.Implementation;
using Monitoring.Core.Validators;

namespace Monitoring.Core
{
    public static class ConfigureMonitoringDependenciesExtension
    {
        public static IServiceCollection AddMonitoringDependencies( this IServiceCollection services )
        {
            // Services
            services.AddScoped<ISynchronizerService, SynchronizerService>();

            // Builders
            services.AddScoped<IProjectMonitoringBuilder, ProjectMonitoringBuilder>();
            services.AddScoped<ITelegramHandlerBuilder, TelegramHandlerBuilder>();

            // Validators
            services.AddScoped<IValidator<ProjectConfiguration>, ProjectConfigurationValidator>();
            services.AddScoped<IValidator<MonitoringConfiguration>, MonitoringConfigurationValidator>();
            services.AddScoped<IValidator<TelegramBotConfiguration>, TelegramBotConfigurationValidator>();

            return services;
        }
    }
}