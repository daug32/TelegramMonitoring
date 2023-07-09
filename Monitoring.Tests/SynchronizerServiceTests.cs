using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Builders;
using Monitoring.Core.Implementation.Services;
using Monitoring.Core.Implementation.Validators;
using Moq;
using NUnit.Framework;

namespace Monitoring.Tests;

public class SynchronizerServiceTests
{
    private string _monitoringMessage;
    private string _sentMessage;
    private SynchronizerService _synchronizerService;

    [SetUp]
    public void SetUp()
    {
        // ProjectMonitoringBuilder
        var projectMonitoringMock = new Mock<IProjectMonitoring>();
        projectMonitoringMock
            .Setup( monitoring => monitoring.GetMessageFromProjectAsync() )
            .ReturnsAsync( () => _monitoringMessage );

        var projectMonitoringBuilderMock = new Mock<IProjectMonitoringBuilder>();
        projectMonitoringBuilderMock
            .Setup( builder => builder.Build( It.IsAny<AppMonitoringConfiguration>() ) )
            .Returns( projectMonitoringMock.Object );

        // TelegramHandlerBuilder
        var telegramHandlerMock = new Mock<ITelegramHandler>();
        telegramHandlerMock
            .Setup( handler => handler.SendMessageAsync( It.IsAny<string>() ) )
            .Returns( Task.Run( () => { } ) )
            .Callback( ( string message ) => _sentMessage = message );

        var telegramHandlerBuilderMoq = new Mock<ITelegramHandlerBuilder>();
        telegramHandlerBuilderMoq
            .Setup( builder => builder.Build(
                It.IsAny<TelegramBotConfiguration>(),
                It.IsAny<TelegramChatConfiguration>() ) )
            .Returns( telegramHandlerMock.Object );

        // SynchronizerService
        _synchronizerService = new SynchronizerService(
            projectMonitoringBuilderMock.Object,
            telegramHandlerBuilderMoq.Object,
            new Mock<IValidator<ProjectConfiguration>>().Object );
    }

    [Test]
    public void NotifySingleProjectAsync_MonitoringReturnedNotEmptyMessage_MessageIsSentToTelegram()
    {
        // Arrange
        var config = new ProjectConfiguration
        {
            ProjectName = "Test project",
            AppMonitoringConfiguration = new AppMonitoringConfiguration(),
            TelegramChatConfiguration = new TelegramChatConfiguration()
        };

        _monitoringMessage = "Success";

        // Act
        _synchronizerService.NotifySingleProjectAsync( config ).Wait();

        // Assert
        Assert.IsTrue( _sentMessage.Contains( config.ProjectName ) );
        Assert.IsTrue( _sentMessage.Contains( _monitoringMessage ) );
    }

    [Test]
    public void NotifySingleProjectAsync_MonitoringReturnedEmptyMessageAndProjectConfiguredToSendMessagesInThisCase_MessageIsSent()
    {
        // Arrange
        var config = new ProjectConfiguration
        {
            ProjectName = "Test project",
            NotifyIfMonitoringReturnedEmptyMessage = true,
            AppMonitoringConfiguration = new AppMonitoringConfiguration(),
            TelegramChatConfiguration = new TelegramChatConfiguration()
        };

        _monitoringMessage = "";

        // Act
        _synchronizerService.NotifySingleProjectAsync( config ).Wait();

        // Assert
        Assert.IsFalse( String.IsNullOrEmpty( _sentMessage ) );
    }

    [Test]
    public void NotifySingleProjectAsync_MonitoringReturnedEmptyMessageAndProjectConfiguredToNotSendMessagesInThisCase_NoMessageIsSent()
    {
        // Arrange
        var config = new ProjectConfiguration
        {
            ProjectName = "Test project",
            NotifyIfMonitoringReturnedEmptyMessage = false,
            AppMonitoringConfiguration = new AppMonitoringConfiguration(),
            TelegramChatConfiguration = new TelegramChatConfiguration()
        };

        _monitoringMessage = "";

        // Act
        _synchronizerService.NotifySingleProjectAsync( config ).Wait();

        // Assert
        Assert.IsTrue( String.IsNullOrEmpty( _sentMessage ) );
    }
}