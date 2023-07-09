using System.Threading.Tasks;

namespace Monitoring.Core.Services
{
    public interface ITelegramHandler
    {
        Task SendMessageAsync( string message );
    }
}