using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface IProjectNotificator
    {
        Task NotifyAllProjectsAsync( IEnumerable<ProjectConfiguration> projects );
        
        Task NotifyAllProjectsAsync(
            IEnumerable<ProjectConfiguration> projects, 
            CancellationToken cancellationToken );
        
        Task NotifyProjectAsync( ProjectConfiguration project );

        Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken cancellationToken );
    }
}