using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Core.Configurations;

namespace Monitoring.Core.Implementation.Telegram
{
    internal interface ITelegramHandlerBuilder
    {
        ITelegramHandler Build(
            TelegramBotConfiguration botConfiguration,
            TelegramChatConfiguration chatConfiguration );
    }
    
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
}