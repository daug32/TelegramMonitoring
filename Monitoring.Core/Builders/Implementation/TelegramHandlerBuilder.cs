using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Configurations;
using Monitoring.Core.Services;
using Monitoring.Core.Services.Implementation;

namespace Monitoring.Core.Builders.Implementation;

internal class TelegramHandlerBuilder : ITelegramHandlerBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public TelegramHandlerBuilder( IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
    }

    public ITelegramHandler Build(
        TelegramBotConfiguration botConfiguration,
        TelegramChatConfiguration chatConfiguration )
    {
        var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
        
        return new TelegramHandler(
            botConfiguration.ApiKey,
            chatConfiguration,
            httpClient );
    }
}