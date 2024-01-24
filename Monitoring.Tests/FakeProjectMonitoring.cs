using Monitoring.Core.Implementation.Monitorings;

namespace Monitoring.Tests
{
    public class FakeProjectMonitoring : IProjectMonitoring
    {
        public string MessageThatWillBeSent { get; set; }

        public Task<string> GetMessageFromProjectAsync( CancellationToken cancellationToken )
        {
            return Task.FromResult( MessageThatWillBeSent );
        }
    }
}