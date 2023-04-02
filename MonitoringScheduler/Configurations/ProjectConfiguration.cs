namespace MonitoringScheduler.Configurations;

public class ProjectConfiguration
{
    public string ProjectName { get; set; }
    public TelegramChatConfiguration TelegramChatConfiguration { get; set; }
    public MonitoringConfiguration MonitoringConfiguration { get; set; }
}