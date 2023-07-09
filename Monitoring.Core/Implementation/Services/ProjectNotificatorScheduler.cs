using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Models;

namespace Monitoring.Core.Implementation.Services
{
    internal class ProjectNotificatorScheduler : IProjectNotificatorScheduler
    {
        private readonly IProjectNotificator _projectNotificator;

        public ProjectNotificatorScheduler( IProjectNotificator projectNotificator )
        {
            _projectNotificator = projectNotificator;
        }

        public async Task ScheduleNotificationAsync(
            IEnumerable<ProjectConfiguration> projects,
            CancellationToken cancellationToken )
        {
            var tasks = new List<Task>();
            var projectsQueue = new SafeConcurrentQueue<ProjectConfiguration>( projects );

            while ( !cancellationToken.IsCancellationRequested )
            {
                while ( !projectsQueue.IsEmpty )
                {
                    ProjectConfiguration project = projectsQueue.Pop();
                    if ( project == null )
                    {
                        continue;
                    }

                    Task task = BuildNotificationTask( project, cancellationToken )
                        .ContinueWith( _ => projectsQueue.Push( project ), cancellationToken );

                    tasks.Add( task );
                }

                Task completedTask = await Task.WhenAny( tasks );
                tasks.Remove( completedTask );
            }
        }

        private Task BuildNotificationTask(
            ProjectConfiguration project,
            CancellationToken cancellationToken )
        {
            Task task = Task
                .Run(
                    () =>
                    {
                        Task notificationTask = _projectNotificator.NotifyProjectAsync( project, cancellationToken );
                        Task waitTask = Task.Delay( project.Delay, cancellationToken );

                        return Task.WhenAll( notificationTask, waitTask );
                    },
                    cancellationToken );

            return task;
        }
    }
}