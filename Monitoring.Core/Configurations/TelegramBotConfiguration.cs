using System;

namespace Monitoring.Core.Configurations
{
    public class TelegramBotConfiguration
    {
        public string ApiKey { get; set; }
        
        public static void ValidateOrThrow( TelegramBotConfiguration config )
        {
            if ( config == null )
            {
                throw new ArgumentNullException( nameof( config ) );
            }

            if ( String.IsNullOrWhiteSpace( config.ApiKey ) )
            {
                throw new ArgumentNullException(
                    nameof( config.ApiKey ),
                    "Bot api key can't be null or empty" );
            }
        }
    }
}