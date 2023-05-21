﻿using MonitoringScheduler.Configurations;
using MonitoringScheduler.Services;
using MonitoringScheduler.Services.Builders;
using MonitoringScheduler.Services.Implementation;
using Moq;
using NUnit.Framework;

namespace MonitoringScheduler.Tests;

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
            .Setup( builder => builder.Build( It.IsAny<MonitoringConfiguration>() ) )
            .Returns( projectMonitoringMock.Object );

        // TelegramHandlerBuilder
        var telegramHandlerMock = new Mock<ITelegramHandler>();
        telegramHandlerMock
            .Setup( handler => handler.SendMessageAsync( It.IsAny<string>() ) )
            .Returns( Task.Run( () => { } ) )
            .Callback( ( string message ) => _sentMessage = message );

        var telegramHandlerBuilderMoq = new Mock<ITelegramHandlerBuilder>();
        telegramHandlerBuilderMoq
            .Setup( builder => builder.Build( It.IsAny<TelegramChatConfiguration>() ) )
            .Returns( telegramHandlerMock.Object );

        // SynchronizerService
        _synchronizerService = new SynchronizerService(
            projectMonitoringBuilderMock.Object,
            telegramHandlerBuilderMoq.Object );
    }

    [Test]
    public void NotifySingleProjectAsync_MonitoringReturnedNotEmptyMessage_MessageIsSentToTelegram()
    {
        // Arrange
        var config = new ProjectConfiguration
        {
            ProjectName = "Test project",
            MonitoringConfiguration = new MonitoringConfiguration(),
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
    public void NotifySingleProjectAsync_MonitoringReturnedEmptyMessage_NoMessageIsSentToTelegram()
    {
        // Arrange
        var config = new ProjectConfiguration
        {
            ProjectName = "Test project",
            MonitoringConfiguration = new MonitoringConfiguration(),
            TelegramChatConfiguration = new TelegramChatConfiguration()
        };

        _monitoringMessage = "";

        // Act
        _synchronizerService.NotifySingleProjectAsync( config ).Wait();

        // Assert
        Assert.IsTrue( String.IsNullOrEmpty( _monitoringMessage ) );
    }
}