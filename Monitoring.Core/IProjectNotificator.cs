using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface IProjectNotificator
    {
        Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken token = default );
    }
}