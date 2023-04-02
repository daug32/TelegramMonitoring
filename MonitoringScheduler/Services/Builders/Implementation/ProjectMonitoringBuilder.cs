using MonitoringScheduler.Configurations;
using MonitoringScheduler.Services.Implementation;

namespace MonitoringScheduler.Services.Builders.Implementation;

internal class ProjectMonitoringBuilder : IProjectMonitoringBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public ProjectMonitoringBuilder( IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
    }

    public IProjectMonitoring Build( MonitoringConfiguration configuration )
    {
        var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
        return new ProjectMonitoring( httpClient, configuration );
    }
}