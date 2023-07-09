using System;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Implementation.Validators
{
    public class MonitoringConfigurationValidator : IValidator<AppMonitoringConfiguration>
    {
        public void ValidateOrThrow( AppMonitoringConfiguration config )
        {
            if ( config == null )
            {
                throw new ArgumentNullException( nameof( config ) );
            }

            if ( String.IsNullOrWhiteSpace( config.Url ) )
            {
                throw new ArgumentNullException(
                    nameof( config.Url ),
                    "Monitoring URL can't be null or empty" );
            }
        }
    }
}