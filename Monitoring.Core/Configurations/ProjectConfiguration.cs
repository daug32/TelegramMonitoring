using System;

namespace Monitoring.Core.Configurations
{
    public class ProjectConfiguration
    {
        public string ProjectName { get; set; }

        /// <summary>
        ///     If true, sends a message that the monitoring returned an empty message.
        ///     If false, no message is sent in these case.
        /// </summary>
        public bool NotifyIfMonitoringReturnedEmptyMessage { get; set; } = false;

        public TimeSpan Delay { get; set; }

        public TelegramBotConfiguration TelegramBotConfiguration { get; set; }

        public TelegramChatConfiguration TelegramChatConfiguration { get; set; }

        public AppMonitoringConfiguration AppMonitoringConfiguration { get; set; }

        public static void ValidateOrThrow( ProjectConfiguration config )
        {
            if ( config == null )
            {
                throw new ArgumentNullException( nameof( config ) );
            }

            if ( String.IsNullOrWhiteSpace( config.ProjectName ) )
            {
                throw new ArgumentNullException(
                    nameof( config.ProjectName ),
                    "Project name can't be null or empty" );
            }
            
            AppMonitoringConfiguration.ValidateOrThrow( config.AppMonitoringConfiguration );
            TelegramBotConfiguration.ValidateOrThrow( config.TelegramBotConfiguration );
        }
    }
}