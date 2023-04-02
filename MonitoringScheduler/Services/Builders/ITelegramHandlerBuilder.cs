using MonitoringScheduler.Configurations;

namespace MonitoringScheduler.Services.Builders;

public interface ITelegramHandlerBuilder
{
    ITelegramHandler Build( TelegramChatConfiguration chatConfiguration );
}