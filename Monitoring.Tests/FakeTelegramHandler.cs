﻿using Monitoring.Core.Implementation.Services;

namespace Monitoring.Tests
{
    public class FakeTelegramHandler : ITelegramHandler
    {
        public string LastSentMessage { get; set; } = null;

        public Task SendMessageAsync( string message, CancellationToken cancellationToken )
        {
            LastSentMessage = message;
            return Task.CompletedTask;
        }
    }
}