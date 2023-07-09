using System;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Validators
{
    public class TelegramBotConfigurationValidator : IValidator<TelegramBotConfiguration>
    {
        public void ValidateOrThrow( TelegramBotConfiguration botConfiguration )
        {
            if ( botConfiguration == null )
            {
                throw new ArgumentNullException( nameof( botConfiguration ) );
            }

            if ( String.IsNullOrWhiteSpace( botConfiguration.ApiKey ) )
            {
                throw new ArgumentNullException(
                    nameof( botConfiguration.ApiKey ),
                    "Bot api key can't be null or empty" );
            }
        }
    }
}