using Monitoring.Core.Configurations;
using Monitoring.Core.Services;

namespace Monitoring.BackgroundService;

public class MonitoringScheduler : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private ISynchronizerService _synchronizerService;

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
        // We need to get in code
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        _synchronizerService = scope.ServiceProvider.GetRequiredService<ISynchronizerService>();
            
        while ( !cancellationToken.IsCancellationRequested )
        {
            Task delay = Task.Delay( ParseDelay(), cancellationToken );
            
            await NotifyProjectsAsync();
            
            await delay.WaitAsync( cancellationToken );
        }
    }

    private Task NotifyProjectsAsync()
    {
        List<ProjectConfiguration> projectConfigurations = _configuration
            .GetSection( "ProjectConfigurations" )
            .Get<List<ProjectConfiguration>>();

        return _synchronizerService.NotifyAllProjectsAsync( projectConfigurations );
    }

    private TimeSpan ParseDelay()
    {
        var minutes = _configuration
            .GetSection( "Scheduling:DelayInMinutes" )
            .Get<int>();

        return new TimeSpan( 0, minutes, 0 );
    }
}