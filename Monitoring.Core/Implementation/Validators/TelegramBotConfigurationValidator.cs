using System;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Implementation.Validators
{
    public class TelegramBotConfigurationValidator : IValidator<TelegramBotConfiguration>
    {
        public void ValidateOrThrow( TelegramBotConfiguration config )
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