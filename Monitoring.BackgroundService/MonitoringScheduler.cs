using System.Collections.Concurrent;
using Monitoring.Core;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation;

namespace Monitoring.BackgroundService;

public class MonitoringScheduler : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MonitoringScheduler(
        IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory )
    {
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync( CancellationToken cancellationToken )
    {
        // Due to Monitoring.BackgroundService is a singleton service and ISynchronizerService is a scoped class
        // We need to get ISynchronizerService in code
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var notificator = scope.ServiceProvider.GetRequiredService<IProjectNotificatorScheduler>();
        
        IEnumerable<ProjectConfiguration> projects = GetProjectConfigurations();
        await notificator.ScheduleNotificationAsync( projects, cancellationToken );
    }

    private IEnumerable<ProjectConfiguration> GetProjectConfigurations()
    {
        return _configuration
            .GetSection( "ProjectConfigurations" )
            .Get<List<ProjectConfiguration>>();
    }
}