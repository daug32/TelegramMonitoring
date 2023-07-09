using Monitoring.Core;
using Monitoring.Core.Configurations;

namespace Monitoring.BackgroundService;

public class MonitoringScheduler : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IProjectNotificator _projectNotificator;

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
        _projectNotificator = scope.ServiceProvider.GetRequiredService<IProjectNotificator>();

        while ( !cancellationToken.IsCancellationRequested )
        {
            IEnumerable<ProjectConfiguration> configurations = GetProjectConfigurations();
            await _projectNotificator.NotifyAllProjectsAsync( 
                configurations,
                cancellationToken );
        }
    }

    private IEnumerable<ProjectConfiguration> GetProjectConfigurations()
    {
        return _configuration
            .GetSection( "ProjectConfigurations" )
            .Get<List<ProjectConfiguration>>();
    }
}