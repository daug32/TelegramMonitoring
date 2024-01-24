namespace Monitoring.BackgroundService
{
    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host
                .CreateDefaultBuilder()
                .ConfigureServices( ( _, services ) =>
                {
                    services
                        .ConfigureDependencies()
                        .AddHostedService<MonitoringScheduler>();
                } );
        }
    }
}