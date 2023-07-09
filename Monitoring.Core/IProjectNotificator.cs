using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface IProjectNotificator
    {
        Task NotifyAllProjectsAsync( IEnumerable<ProjectConfiguration> projects );
        Task NotifyProjectAsync( ProjectConfiguration project );
    }
}