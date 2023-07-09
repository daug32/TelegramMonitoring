using System;

namespace Monitoring.Core.Configurations
{
    public class ProjectConfiguration
    {
        public string ProjectName { get; set; }

        /// <summary>
        ///     If true, sends a message that the monitoring returned an empty message.
        ///     If false, no message is sent in these cases.
        /// </summary>
        public bool NotifyIfMonitoringReturnedEmptyMessage { get; set; } = false;

        public TimeSpan Delay { get; set; }

        public TelegramBotConfiguration TelegramBotConfiguration { get; set; }

        public TelegramChatConfiguration TelegramChatConfiguration { get; set; }

        public AppMonitoringConfiguration AppMonitoringConfiguration { get; set; }
    }
}