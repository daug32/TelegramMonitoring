using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core
{
    public interface ISynchronizerService
    {
        Task NotifySingleProjectAsync( ProjectConfiguration project );
        Task NotifyAllProjectsAsync( IEnumerable<ProjectConfiguration> projects );
    }
}