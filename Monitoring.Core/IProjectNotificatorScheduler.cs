using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface IProjectNotificatorScheduler
    {
        Task ScheduleNotificationAsync(
            IEnumerable<ProjectConfiguration> projects,
            CancellationToken token );
    }
}