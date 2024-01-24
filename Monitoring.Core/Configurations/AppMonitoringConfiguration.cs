using System;

namespace Monitoring.Core.Configurations
{
    public class AppMonitoringConfiguration
    {
        public string Url { get; set; }
        public string AuthenticationToken { get; set; } = string.Empty;
        public string AuthenticationTokenHeader { get; set; } = string.Empty;
        
        public static void ValidateOrThrow( AppMonitoringConfiguration config )
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

            try
            {
                new Uri( config.Url );
            }
            catch
            {
                throw new ArgumentException(
                    nameof( config.Url ),
                    $"Invalid url. Provided url: {config.Url}" );
            }
        }
    }
}