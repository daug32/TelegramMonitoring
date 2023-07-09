using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Monitoring.Core.Builders;
using Monitoring.Core.Configurations;
using Monitoring.Core.Validators;

namespace Monitoring.Core.Services.Implementation
{
    internal class SynchronizerService : ISynchronizerService
    {
        private readonly IValidator<ProjectConfiguration> _projectConfigurationValidator;
        private readonly IProjectMonitoringBuilder _projectMonitoringBuilder;
        private readonly ITelegramHandlerBuilder _telegramHandlerBuilder;

        public SynchronizerService(
            IProjectMonitoringBuilder projectMonitoringBuilder,
            ITelegramHandlerBuilder telegramHandlerBuilder,
            IValidator<ProjectConfiguration> projectConfigurationValidator )
        {
            _projectMonitoringBuilder = projectMonitoringBuilder;
            _telegramHandlerBuilder = telegramHandlerBuilder;
            _projectConfigurationValidator = projectConfigurationValidator;
        }

        public async Task NotifyAllProjectsAsync( IEnumerable<ProjectConfiguration> projects )
        {
            foreach ( ProjectConfiguration project in projects )
            {
                await NotifySingleProjectAsync( project );
            }
        }

        public async Task NotifySingleProjectAsync( ProjectConfiguration project )
        {
            _projectConfigurationValidator.ValidateOrThrow( project );

            ITelegramHandler telegramHandler = _telegramHandlerBuilder.Build(
                project.TelegramBotConfiguration,
                project.TelegramChatConfiguration );

            string message;
            try
            {
                message = await _projectMonitoringBuilder
                    .Build( project.MonitoringConfiguration )
                    .GetMessageFromProjectAsync();
            }
            catch ( HttpRequestException ex )
            {
                string errorMessage = BuildRequestErrorMessage( 
                    project.ProjectName,
                    ex.Message );
                await telegramHandler.SendMessageAsync( errorMessage );
                return;
            }

            if ( string.IsNullOrWhiteSpace( message )
                 && !project.NotifyIfMonitoringReturnedEmptyMessage )
            {
                return;
            }

            await telegramHandler.SendMessageAsync( BuildMessage( project.ProjectName, message ) );
        }

        private static string BuildMessage( string projectName, string message )
        {
            return $"Application: \"{projectName}\". Message:\n{message}";
        }

        private static string BuildRequestErrorMessage( string projectName, string exceptionMessage )
        {
            return 
                $"Application: \"{projectName}\". Couldn't get message from application. Message:\n{exceptionMessage}";
        }
    }
}