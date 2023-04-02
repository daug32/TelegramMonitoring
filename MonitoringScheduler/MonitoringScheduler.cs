using MonitoringScheduler.Configurations;
using MonitoringScheduler.Services;

namespace MonitoringScheduler;

public class MonitoringScheduler : BackgroundService
{
    private readonly TimeSpan _delay;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private ISynchronizerService _synchronizerService;

    public MonitoringScheduler(
        IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory )
    {
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
        _delay = ParseDelay();
    }

    protected override async Task ExecuteAsync( CancellationToken cancellationToken )
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        _synchronizerService = scope.ServiceProvider.GetRequiredService<ISynchronizerService>();
            
        while ( !cancellationToken.IsCancellationRequested )
        {
            Task delay = Task.Delay( _delay, cancellationToken );
            
            await NotifyProjectsAsync();
            
            delay.Wait( cancellationToken );
        }
    }

    private Task NotifyProjectsAsync()
    {
        var projectConfigurations = _configuration
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