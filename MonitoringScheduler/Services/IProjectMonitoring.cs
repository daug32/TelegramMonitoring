namespace MonitoringScheduler.Services;

public interface IProjectMonitoring
{
    Task<string> GetMessageFromProjectAsync();
}