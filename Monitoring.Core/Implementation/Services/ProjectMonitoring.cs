using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Implementation.Services
{
    internal interface IProjectMonitoring
    {
        Task<string> GetMessageFromProjectAsync();
        Task<string> GetMessageFromProjectAsync( CancellationToken cancellationToken );
    }
    
    internal class ProjectMonitoring : IProjectMonitoring
    {
        private readonly AppMonitoringConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ProjectMonitoring(
            HttpClient httpClient,
            AppMonitoringConfiguration configuration )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public Task<string> GetMessageFromProjectAsync()
        {
            return GetMessageFromProjectAsync( CancellationToken.None );
        }

        public async Task<string> GetMessageFromProjectAsync( CancellationToken cancellationToken )
        {
            var request = new HttpRequestMessage( HttpMethod.Get, _configuration.Url );
            if ( !String.IsNullOrWhiteSpace( _configuration.AuthenticationTokenHeader ) )
            {
                request.Headers.Add( _configuration.AuthenticationTokenHeader, _configuration.AuthenticationToken );
            }

            HttpResponseMessage response = await _httpClient.SendAsync( request, cancellationToken );
            string result = await response.Content.ReadAsStringAsync();
            if ( !response.IsSuccessStatusCode )
            {
                throw new HttpRequestException(
                    $"Url: {_configuration.Url}\nStatusCode: {response.StatusCode}\nResult: {result}" );
            }

            return result;
        }
    }
}