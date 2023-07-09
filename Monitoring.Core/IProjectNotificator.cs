using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface IProjectNotificator
    {
        Task NotifyAllProjectsAsync(
            IEnumerable<ProjectConfiguration> projects, 
            CancellationToken? cancellationToken = null );
        Task NotifyProjectAsync( 
            ProjectConfiguration project,
            CancellationToken? cancellationToken = null );
    }
}