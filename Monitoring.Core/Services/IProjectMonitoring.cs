using System.Threading.Tasks;

namespace Monitoring.Core.Services
{
    public interface IProjectMonitoring
    {
        Task<string> GetMessageFromProjectAsync();
    }
}