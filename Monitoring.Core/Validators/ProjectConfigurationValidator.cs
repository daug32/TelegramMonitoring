using Monitoring.Core.Configurations;

namespace Monitoring.Core.Validators;

public class ProjectConfigurationValidator : IValidator<ProjectConfiguration>
{
    private readonly IValidator<MonitoringConfiguration> _monitoringValidator;
    private readonly IValidator<TelegramBotConfiguration> _botValidator;

    public ProjectConfigurationValidator( 
        IValidator<MonitoringConfiguration> monitoringValidator, 
        IValidator<TelegramBotConfiguration> botValidator )
    {
        _monitoringValidator = monitoringValidator;
        _botValidator = botValidator;
    }

    public void ValidateOrThrow( ProjectConfiguration projectConfiguration )
    {
        if ( projectConfiguration == null )
        {
            throw new ArgumentNullException( nameof( projectConfiguration ) );
        }
        
        if ( String.IsNullOrWhiteSpace( projectConfiguration.ProjectName ) )
        {
            throw new ArgumentNullException(
                nameof( projectConfiguration.ProjectName ),
                "Project name can't be null or empty" );
        }
        
        _botValidator.ValidateOrThrow( projectConfiguration.TelegramBotConfiguration );
        _monitoringValidator.ValidateOrThrow( projectConfiguration.MonitoringConfiguration );
    }
}