using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public ProjectNotificator(
            IProjectMonitoringBuilder projectMonitoringBuilder,
            ITelegramHandlerBuilder telegramHandlerBuilder,
            ILogger<ProjectNotificator> logger )
        {
            _projectMonitoringBuilder = projectMonitoringBuilder;
            _telegramHandlerBuilder = telegramHandlerBuilder;
            _logger = logger;
        }

        public async Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken token = default )
        {
            ProjectConfiguration.ValidateOrThrow( project );

            ITelegramHandler telegramHandler = _telegramHandlerBuilder.Build(
                project.TelegramBotConfiguration,
                project.TelegramChatConfiguration );

            _logger.Log( LogLevel.Information, $"Asking for message. Project: {project.ProjectName}" );

            string message;
            try
            {
                message = await _projectMonitoringBuilder
                    .Build( project.AppMonitoringConfiguration )
                    .GetMessageFromProjectAsync( token );
            }
            catch ( HttpRequestException ex )
            {
                message = ex.Message;
            }

            if ( string.IsNullOrWhiteSpace( message ) &&
                 !project.NotifyIfMonitoringReturnedEmptyMessage )
            {
                return;
            }

            _logger.Log( LogLevel.Information, $"Sends a message. Project: {project.ProjectName}" );
            await telegramHandler.SendMessageAsync(
                MessageCreator.Create( 
                    projectName: project.ProjectName, 
                    message: message ),
                token );
        }
    }
}