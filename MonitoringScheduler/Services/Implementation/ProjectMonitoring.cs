using MonitoringScheduler.Configurations;

namespace MonitoringScheduler.Services.Implementation;

internal class ProjectMonitoring : IProjectMonitoring
{
    private readonly MonitoringConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ProjectMonitoring(
        HttpClient httpClient,
        MonitoringConfiguration configuration )
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetMessageFromProjectAsync()
    {
        var request = new HttpRequestMessage( HttpMethod.Get, _configuration.Url );
        request.Headers.Add( _configuration.AuthenticationTokenHeader, _configuration.AuthenticationToken );

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