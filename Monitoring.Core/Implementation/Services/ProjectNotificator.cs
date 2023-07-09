using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Builders;
using Monitoring.Core.Implementation.Utils;
using Monitoring.Core.Implementation.Validators;

namespace Monitoring.Core.Implementation.Services
{
    internal class ProjectNotificator : IProjectNotificator
    {
        private readonly IValidator<ProjectConfiguration> _projectConfigurationValidator;
        private readonly IProjectMonitoringBuilder _projectMonitoringBuilder;
        private readonly ITelegramHandlerBuilder _telegramHandlerBuilder;

        public ProjectNotificator(
            IProjectMonitoringBuilder projectMonitoringBuilder,
            ITelegramHandlerBuilder telegramHandlerBuilder,
            IValidator<ProjectConfiguration> projectConfigurationValidator )
        {
            _projectMonitoringBuilder = projectMonitoringBuilder;
            _telegramHandlerBuilder = telegramHandlerBuilder;
            _projectConfigurationValidator = projectConfigurationValidator;
        }

        public Task NotifyProjectAsync( ProjectConfiguration project )
        {
            return NotifyProjectAsync( project, CancellationToken.None );
        }

        public async Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken cancellationToken )
        {
            _projectConfigurationValidator.ValidateOrThrow( project );

            ITelegramHandler telegramHandler = _telegramHandlerBuilder.Build(
                project.TelegramBotConfiguration,
                project.TelegramChatConfiguration );

            string message;
            try
            {
                message = await _projectMonitoringBuilder
                    .Build( project.AppMonitoringConfiguration )
                    .GetMessageFromProjectAsync( cancellationToken );
            }
            catch ( HttpRequestException ex )
            {
                string errorMessage = MessageBuilder.BuildRequestErrorMessage(
                    project.ProjectName,
                    ex.Message );
                await telegramHandler.SendMessageAsync( errorMessage, cancellationToken );
                return;
            }

            if ( string.IsNullOrWhiteSpace( message ) &&
                 !project.NotifyIfMonitoringReturnedEmptyMessage )
            {
                return;
            }

            await telegramHandler.SendMessageAsync(
                MessageBuilder.BuildMessage( project.ProjectName, message ),
                cancellationToken );
        }
    }
}