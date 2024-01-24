using Monitoring.Core;
using Monitoring.Core.Configurations;

namespace Monitoring.BackgroundService
{
    public class MonitoringScheduler : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IProjectNotificatorScheduler _notificator;

        public MonitoringScheduler(
            IConfiguration configuration,
            IServiceProvider serviceProvider )
        {
            _configuration = configuration;

            // Due to Monitoring.BackgroundService is a singleton service and IProjectNotificatorScheduler is a scoped class
            // We need to get IProjectNotificatorScheduler in code
            _notificator = serviceProvider.GetRequiredService<IProjectNotificatorScheduler>();
        }

        protected override async Task ExecuteAsync( CancellationToken cancellationToken )
        {
            List<ProjectConfiguration> projects = GetProjectConfigurations();
            await _notificator.ScheduleNotificationAsync( projects, cancellationToken );
        }

        private List<ProjectConfiguration> GetProjectConfigurations()
        {
            List<ProjectConfiguration> configs = _configuration
                .GetSection( "ProjectConfigurations" )
                .Get<List<ProjectConfiguration>>();

            if ( configs == null )
            {
                throw new ArgumentNullException( nameof( configs ) );
            }

            foreach ( ProjectConfiguration config in configs )
            {
                ProjectConfiguration.ValidateOrThrow( config );
            }

            return configs;
        }
    }
}