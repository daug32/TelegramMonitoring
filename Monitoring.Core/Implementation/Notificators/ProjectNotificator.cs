using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Monitorings;
using Monitoring.Core.Implementation.Notificators.Utils;
using Monitoring.Core.Implementation.Telegram;

namespace Monitoring.Core.Implementation.Notificators
{
    internal class ProjectNotificator : IProjectNotificator
    {
        private readonly IProjectMonitoringBuilder _projectMonitoringBuilder;
        private readonly ITelegramHandlerBuilder _telegramHandlerBuilder;

        public ProjectNotificator(
            IProjectMonitoringBuilder projectMonitoringBuilder,
            ITelegramHandlerBuilder telegramHandlerBuilder )
        {
            _projectMonitoringBuilder = projectMonitoringBuilder;
            _telegramHandlerBuilder = telegramHandlerBuilder;
        }

        public Task NotifyProjectAsync( ProjectConfiguration project )
        {
            return NotifyProjectAsync( project, CancellationToken.None );
        }

        public async Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken cancellationToken )
        {
            ProjectConfiguration.ValidateOrThrow( project );

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