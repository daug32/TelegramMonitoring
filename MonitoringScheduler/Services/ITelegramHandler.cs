namespace MonitoringScheduler.Services;

public interface ITelegramHandler
{
    Task SendMessageAsync( string message );
}