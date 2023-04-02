using MonitoringScheduler.Configurations;

namespace MonitoringScheduler.Services.Builders;

public interface IProjectMonitoringBuilder
{
    IProjectMonitoring Build( MonitoringConfiguration configuration );
}