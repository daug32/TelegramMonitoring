using Monitoring.Core.Configurations;
using Monitoring.Core.Services;

namespace Monitoring.Core.Builders;

public interface ITelegramHandlerBuilder
{
    ITelegramHandler Build(
        TelegramBotConfiguration botConfiguration,
        TelegramChatConfiguration chatConfiguration );
}