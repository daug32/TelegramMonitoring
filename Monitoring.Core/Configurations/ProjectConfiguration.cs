namespace Monitoring.Core.Configurations;

public class ProjectConfiguration
{
    public string ProjectName { get; set; }
    public TelegramBotConfiguration TelegramBotConfiguration { get; set; }
    public TelegramChatConfiguration TelegramChatConfiguration { get; set; }
    public MonitoringConfiguration MonitoringConfiguration { get; set; }
}