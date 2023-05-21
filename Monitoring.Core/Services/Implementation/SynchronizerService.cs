using System.Runtime.CompilerServices;
using Monitoring.Core.Builders;
using Monitoring.Core.Configurations;

[assembly: InternalsVisibleTo( "Monitoring.Tests" )]

namespace Monitoring.Core.Services.Implementation;

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

    public async Task NotifyAllProjectsAsync( IEnumerable<ProjectConfiguration> projects )
    {
        foreach ( ProjectConfiguration project in projects )
        {
            await NotifySingleProjectAsync( project );
        }
    }

    public async Task NotifySingleProjectAsync( ProjectConfiguration project )
    {
        ITelegramHandler telegramHandler = _telegramHandlerBuilder.Build(
            project.TelegramBotConfiguration,
            project.TelegramChatConfiguration );

        string message;
        try
        {
            message = await _projectMonitoringBuilder
                .Build( project.MonitoringConfiguration )
                .GetMessageFromProjectAsync();
        }
        catch ( HttpRequestException )
        {
            await telegramHandler.SendMessageAsync( BuildRequestErrorMessage( project.ProjectName ) );
            return;
        }

        if ( string.IsNullOrWhiteSpace( message ) )
        {
            return;
        }

        await telegramHandler.SendMessageAsync( BuildMessage( project.ProjectName, message ) );
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