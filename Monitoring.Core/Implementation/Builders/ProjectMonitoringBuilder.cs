using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Services;

namespace Monitoring.Core.Implementation.Builders
{
    internal interface IProjectMonitoringBuilder
    {
        IProjectMonitoring Build( AppMonitoringConfiguration configuration );
    }
    
    internal class ProjectMonitoringBuilder : IProjectMonitoringBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public ProjectMonitoringBuilder( IServiceProvider serviceProvider )
        {
            _serviceProvider = serviceProvider;
        }

        public IProjectMonitoring Build( AppMonitoringConfiguration configuration )
        {
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new ProjectMonitoring( httpClient, configuration );
        }
    }
}