namespace MonitoringScheduler;

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
            .ConfigureServices( (hostContext, services) =>
            {
                services
                    .ConfigureDependencies( hostContext.Configuration )
                    .AddHostedService<MonitoringScheduler>();
            } );
    }
}