using System.Runtime.CompilerServices;
using MonitoringScheduler.Configurations;
using MonitoringScheduler.Services.Builders;

[assembly: InternalsVisibleTo( "MonitoringScheduler.Tests" )]

namespace MonitoringScheduler.Services.Implementation;

internal class SynchronizerService : ISynchronizerService
{
    private readonly IProjectMonitoringBuilder _projectMonitoringBuilder;
    private readonly ITelegramHandlerBuilder _telegramHandlerBuilder;

    public SynchronizerService(
        IProjectMonitoringBuilder projectMonitoringBuilder,
        ITelegramHandlerBuilder telegramHandlerBuilder )
    {
        _projectMonitoringBuilder = projectMonitoringBuilder;
        _telegramHandlerBuilder = telegramHandlerBuilder;
    }

    public async Task NotifyAllProjectsAsync( List<ProjectConfiguration> configurations )
    {
        foreach ( ProjectConfiguration configuration in configurations )
        {
            await NotifySingleProjectAsync( configuration );
        }
    }

    public async Task NotifySingleProjectAsync( ProjectConfiguration configuration )
    {
        ITelegramHandler telegramHandler = _telegramHandlerBuilder.Build( configuration.TelegramChatConfiguration );

        string message;
        try
        {
            message = await _projectMonitoringBuilder
                .Build( configuration.MonitoringConfiguration )
                .GetMessageFromProjectAsync();
        }
        catch ( HttpRequestException )
        {
            await telegramHandler.SendMessageAsync( BuildRequestErrorMessage( configuration.ProjectName ) );
            return;
        }

        if ( string.IsNullOrWhiteSpace( message ) )
        {
            return;
        }

        await telegramHandler.SendMessageAsync( BuildMessage( configuration.ProjectName, message ) );
    }

    private static string BuildMessage( string projectName, string message )
    {
        return $"Application: \"{projectName}\". Message:\n{message}";
    }

    private static string BuildRequestErrorMessage( string projectName )
    {
        return $"Application: \"{projectName}\". Couldn't get message from application.";
    }
}