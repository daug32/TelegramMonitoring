using System;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Implementation.Validators
{
    public class ProjectConfigurationValidator : IValidator<ProjectConfiguration>
    {
        private readonly IValidator<AppMonitoringConfiguration> _monitoringValidator;
        private readonly IValidator<TelegramBotConfiguration> _botValidator;

        public ProjectConfigurationValidator(
            IValidator<AppMonitoringConfiguration> monitoringValidator,
            IValidator<TelegramBotConfiguration> botValidator )
        {
            _monitoringValidator = monitoringValidator;
            _botValidator = botValidator;
        }

        public void ValidateOrThrow( ProjectConfiguration config )
        {
            if ( config == null )
            {
                throw new ArgumentNullException( nameof( config ) );
            }

            if ( String.IsNullOrWhiteSpace( config.ProjectName ) )
            {
                throw new ArgumentNullException(
                    nameof( config.ProjectName ),
                    "Project name can't be null or empty" );
            }

            // if ( config.Delay.TotalMinutes < 1 )
            // {
            //     throw new ArgumentException(
            //         "Delay can't be less than 1 minute",
            //         nameof( config.Delay ) );
            // }

            _botValidator.ValidateOrThrow( config.TelegramBotConfiguration );
            _monitoringValidator.ValidateOrThrow( config.AppMonitoringConfiguration );
        }
    }
}