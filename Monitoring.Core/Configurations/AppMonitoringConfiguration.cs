namespace Monitoring.Core.Configurations
{
    public class AppMonitoringConfiguration
    {
        public string Url { get; set; }
        public string AuthenticationToken { get; set; } = string.Empty;
        public string AuthenticationTokenHeader { get; set; } = string.Empty;
    }
}