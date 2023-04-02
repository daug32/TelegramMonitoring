using MonitoringScheduler.Services;
using MonitoringScheduler.Services.Builders;
using MonitoringScheduler.Services.Builders.Implementation;
using MonitoringScheduler.Services.Implementation;

namespace MonitoringScheduler;

public static class ConfigureDependenciesExtension
{
    public static IServiceCollection ConfigureDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration configuration )
    {
        // Services
        serviceCollection.AddScoped<ISynchronizerService, SynchronizerService>();
        
        // Builders
        serviceCollection.AddScoped<IProjectMonitoringBuilder, ProjectMonitoringBuilder>();
        serviceCollection.AddScoped<ITelegramHandlerBuilder, TelegramHandlerBuilder>( serviceProvider =>
        {
            var botApiKey = configuration.GetValue<string>( "TelegramBotApiKey" );
            return new TelegramHandlerBuilder( botApiKey, serviceProvider );
        } );
        
        // Other
        serviceCollection.AddSingleton<HttpClient, HttpClient>();

        return serviceCollection;
    }
}