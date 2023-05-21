using Monitoring.Core.Configurations;
using Telegram.Bot;

namespace Monitoring.Core.Services.Implementation;

internal class TelegramHandler : ITelegramHandler
{
    private const int MaxMessageSize = 4096;
    private readonly TelegramBotClient _telegramClient;
    private readonly TelegramChatConfiguration _chatConfiguration;

    public TelegramHandler(
        string botApiKey,
        TelegramChatConfiguration chatConfiguration,
        HttpClient httpClient )
    {
        _chatConfiguration = chatConfiguration;
        _telegramClient = new TelegramBotClient( botApiKey, httpClient );
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