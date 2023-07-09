using Monitoring.Core.Configurations;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Monitoring.Core.Services.Implementation
{
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

        public async Task<string> GetMessageFromProjectAsync()
        {
            var request = new HttpRequestMessage( HttpMethod.Get, _configuration.Url );
            if ( !String.IsNullOrWhiteSpace( _configuration.AuthenticationTokenHeader ) )
            {
                request.Headers.Add( _configuration.AuthenticationTokenHeader, _configuration.AuthenticationToken );
            }

            HttpResponseMessage response = await _httpClient.SendAsync( request );
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