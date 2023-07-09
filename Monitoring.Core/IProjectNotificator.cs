using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface IProjectNotificator
    {
        Task NotifyProjectAsync( ProjectConfiguration project );

        Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken cancellationToken );
    }
}