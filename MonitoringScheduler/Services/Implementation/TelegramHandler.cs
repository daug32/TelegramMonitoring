using MonitoringScheduler.Configurations;
using Telegram.Bot;

namespace MonitoringScheduler.Services.Implementation;

internal class TelegramHandler : ITelegramHandler
{
    private const int MaxMessageSize = 4096;
    private readonly TelegramBotClient _telegramClient;
    private readonly TelegramChatConfiguration _chatConfiguration;

    public TelegramHandler(
        string apiKey,
        TelegramChatConfiguration chatConfiguration,
        HttpClient httpClient )
    {
        _chatConfiguration = chatConfiguration;
        _telegramClient = new TelegramBotClient( apiKey, httpClient );
    }

    public async Task SendMessageAsync( string message )
    {
        if ( message.Length > MaxMessageSize )
        {
            await _telegramClient.SendTextMessageAsync( _chatConfiguration.ChatId, message[..MaxMessageSize] );
            return;
        }

        await _telegramClient.SendTextMessageAsync( _chatConfiguration.ChatId, message );
    }
}