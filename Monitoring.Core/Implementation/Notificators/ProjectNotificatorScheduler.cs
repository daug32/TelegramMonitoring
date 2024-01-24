using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Notificators.Models;

namespace Monitoring.Core.Implementation.Notificators
{
    internal class ProjectNotificatorScheduler : IProjectNotificatorScheduler
    {
        private readonly IProjectNotificator _projectNotificator;
        private readonly ILogger _logger;  

        public ProjectNotificatorScheduler( 
            IProjectNotificator projectNotificator,
            ILogger<ProjectNotificatorScheduler> logger )
        {
            _projectNotificator = projectNotificator;
            _logger = logger;
        }

        public async Task ScheduleNotificationAsync(
            IEnumerable<ProjectConfiguration> projects,
            CancellationToken token )
        {
            var tasks = new List<Task>();
            var projectsQueue = new SafeConcurrentQueue<ProjectConfiguration>( projects );

            while ( !token.IsCancellationRequested )
            {
                while ( !projectsQueue.IsEmpty )
                {
                    ProjectConfiguration project = projectsQueue.Pop();
                    if ( project == null )
                    {
                        continue;
                    }
                    
                    Task task = Task.Run( async () =>
                        {
                            try
                            {
                                _logger.Log( LogLevel.Information, $"Notifying a project. Project: {project.ProjectName}" );
                                Task notificationTask = _projectNotificator.NotifyProjectAsync( project, token );
                                Task waitTask = Task.Delay( project.Delay, token );
                                await Task.WhenAll( notificationTask, waitTask );
                            }
                            catch ( Exception ex )
                            {
                                _logger.LogCritical( ex, $"Project: {project.ProjectName}" );
                                throw;
                            }
                            finally
                            {
                                projectsQueue.Push( project );
                            }
                        },
                        token );

                    tasks.Add( task );
                }

                if ( !tasks.Any() )
                {
                    continue;
                }

                Task completedTask = await Task.WhenAny( tasks );
                tasks.Remove( completedTask );
            }
        }
    }
}