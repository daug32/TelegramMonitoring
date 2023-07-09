﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Builders;
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

        public Task NotifyAllProjectsAsync(
            IEnumerable<ProjectConfiguration> projects,
            CancellationToken? cancellationToken = null )
        {
            if ( cancellationToken == null )
            {
                cancellationToken = CancellationToken.None;
            }

            Task[] tasks = projects
                .Select( project => Task.Run(
                () =>
                {
                    Task notificationTask = NotifyProjectAsync( project, cancellationToken.Value );
                    Task waitTask = Task.Delay( project.Delay, cancellationToken.Value );

                    return Task.WhenAll( notificationTask, waitTask );
                },
                cancellationToken.Value ) )
                .ToArray(); // Task.WhenAll works more effectively with arrays 

            return Task.WhenAll( tasks );
        }

        public async Task NotifyProjectAsync( 
            ProjectConfiguration project, 
            CancellationToken? cancellationToken = null )
        {
            if ( cancellationToken == null )
            {
                cancellationToken = CancellationToken.None;
            }
            
            _projectConfigurationValidator.ValidateOrThrow( project );

            ITelegramHandler telegramHandler = _telegramHandlerBuilder.Build(
                project.TelegramBotConfiguration,
                project.TelegramChatConfiguration );

            string message;
            try
            {
                message = await _projectMonitoringBuilder
                    .Build( project.AppMonitoringConfiguration )
                    .GetMessageFromProjectAsync( cancellationToken.Value );
            }
            catch ( HttpRequestException ex )
            {
                string errorMessage = BuildRequestErrorMessage(
                    project.ProjectName,
                    ex.Message );
                await telegramHandler.SendMessageAsync( errorMessage, cancellationToken.Value );
                return;
            }

            if ( string.IsNullOrWhiteSpace( message )
                 && !project.NotifyIfMonitoringReturnedEmptyMessage )
            {
                return;
            }

            await telegramHandler.SendMessageAsync(
                BuildMessage( project.ProjectName, message ),
                cancellationToken.Value );
        }

        private static string BuildMessage( string projectName, string message )
        {
            return $"Application: \"{projectName}\".\nMessage: {message}";
        }

        private static string BuildRequestErrorMessage( string projectName, string exceptionMessage )
        {
            return
                $"Application: \"{projectName}\". Couldn't get message from application.\nMessage: {exceptionMessage}";
        }
    }
}