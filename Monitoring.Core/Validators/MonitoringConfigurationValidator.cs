using System;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Validators
{
    public class MonitoringConfigurationValidator : IValidator<MonitoringConfiguration>
    {
        public void ValidateOrThrow( MonitoringConfiguration monitoringConfiguration )
        {
            if ( monitoringConfiguration == null )
            {
                throw new ArgumentNullException( nameof( monitoringConfiguration ) );
            }

            if ( String.IsNullOrWhiteSpace( monitoringConfiguration.Url ) )
            {
                throw new ArgumentNullException(
                    nameof( monitoringConfiguration.Url ),
                    "Monitoring URL can't be null or empty" );
            }
        }
    }
}