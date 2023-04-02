using MonitoringScheduler.Configurations;
using MonitoringScheduler.Services.Implementation;

namespace MonitoringScheduler.Services.Builders.Implementation;

internal class TelegramHandlerBuilder : ITelegramHandlerBuilder
{
    private readonly string _botApiKey;
    private readonly IServiceProvider _serviceProvider;

    public TelegramHandlerBuilder( string botApiKey, IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
        _botApiKey = botApiKey;
    }

    public ITelegramHandler Build( TelegramChatConfiguration chatConfiguration )
    {
        var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
        return new TelegramHandler(
            _botApiKey,
            chatConfiguration,
            httpClient );
    }
}