using Monitoring.Core.Configurations;
using Monitoring.Core.Services;

namespace Monitoring.Core.Builders
{
    public interface IProjectMonitoringBuilder
    {
        IProjectMonitoring Build( AppMonitoringConfiguration configuration );
    }
}