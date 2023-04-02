using MonitoringScheduler.Configurations;

namespace MonitoringScheduler.Services;

public interface ISynchronizerService
{
    Task NotifySingleProjectAsync( ProjectConfiguration configuration );
    Task NotifyAllProjectsAsync( List<ProjectConfiguration> configurations );
}