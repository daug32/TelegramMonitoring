using Microsoft.Extensions.Logging;
using Monitoring.Core.Configurations;
using Monitoring.Core.Implementation.Monitorings;
using Monitoring.Core.Implementation.Notificators;
using Monitoring.Core.Implementation.Telegram;
using Moq;
using NUnit.Framework;

namespace Monitoring.Tests
{
    public class ProjectNotificatorTests
    {
        private ProjectNotificator _projectNotificator;

        private FakeTelegramHandler _telegramHandler;
        private FakeProjectMonitoring _projectMonitoring;

        [SetUp]
        public void SetUp()
        {
            _telegramHandler = new FakeTelegramHandler();
            _projectMonitoring = new FakeProjectMonitoring();
            
            var projectMonitoringBuilderMock = new Mock<IProjectMonitoringBuilder>();
            projectMonitoringBuilderMock
                .Setup( x => x.Build( It.IsAny<AppMonitoringConfiguration>() ) )
                .Returns( () => _projectMonitoring );
            
            var telegramHandlerBuilderMoq = new Mock<ITelegramHandlerBuilder>();
            telegramHandlerBuilderMoq
                .Setup( x => x.Build( It.IsAny<TelegramBotConfiguration>(), It.IsAny<TelegramChatConfiguration>() ) )
                .Returns( () => _telegramHandler );
            
            _projectNotificator = new ProjectNotificator(
                projectMonitoringBuilderMock.Object,
                telegramHandlerBuilderMoq.Object,
                new Mock<ILogger<ProjectNotificator>>().Object );
        }

        [Test]
        public async Task NotifySingleProjectAsync_MonitoringReturnedNotEmptyMessage_MessageIsSentToTelegram()
        {
            // Arrange
            var config = new ProjectConfiguration
            {
                ProjectName = "Test project",
                AppMonitoringConfiguration = new AppMonitoringConfiguration()
                {
                    Url = "http://test.com"
                },
                TelegramChatConfiguration = new TelegramChatConfiguration(),
                TelegramBotConfiguration = new TelegramBotConfiguration()
                {
                    ApiKey = "apikey"
                }
            };

            string messageFromMonitoring = "Success";
            _projectMonitoring.MessageThatWillBeSent = messageFromMonitoring;

            // Act
            await _projectNotificator.NotifyProjectAsync( config );

            // Assert
            string sentMessage = _telegramHandler.LastSentMessage;
            
            Assert.That( sentMessage.Contains( config.ProjectName ), Is.True );
            Assert.That( sentMessage.Contains( messageFromMonitoring ), Is.True );
        }

        [Test]
        public async Task NotifySingleProjectAsync_MonitoringReturnedEmptyMessageAndProjectConfiguredToSendMessagesInThisCase_MessageIsSent()
        {
            // Arrange
            var config = new ProjectConfiguration
            {
                ProjectName = "Test project",
                NotifyIfMonitoringReturnedEmptyMessage = true,
                AppMonitoringConfiguration = new AppMonitoringConfiguration()
                {
                    Url = "http://test.com"
                },
                TelegramChatConfiguration = new TelegramChatConfiguration(),
                TelegramBotConfiguration = new TelegramBotConfiguration()
                {
                    ApiKey = "apikey"
                }
            };

            string messageFromMonitoring = "";
            _projectMonitoring.MessageThatWillBeSent = messageFromMonitoring;

            // Act
            await _projectNotificator.NotifyProjectAsync( config );

            // Assert
            Assert.That( String.IsNullOrEmpty( _telegramHandler.LastSentMessage ), Is.False );
        }

        [Test]
        public async Task NotifySingleProjectAsync_MonitoringReturnedEmptyMessageAndProjectConfiguredToNotSendMessagesInThisCase_NoMessageIsSent()
        {
            // Arrange
            var config = new ProjectConfiguration
            {
                ProjectName = "Test project",
                NotifyIfMonitoringReturnedEmptyMessage = false,
                AppMonitoringConfiguration = new AppMonitoringConfiguration()
                {
                    Url = "http://test.com"
                },
                TelegramChatConfiguration = new TelegramChatConfiguration(),
                TelegramBotConfiguration = new TelegramBotConfiguration()
                {
                    ApiKey = "apikey"
                }
            };

            string messageFromMonitoring = "";
            _projectMonitoring.MessageThatWillBeSent = messageFromMonitoring;

            // Act
            await _projectNotificator.NotifyProjectAsync( config );

            // Assert
            Assert.That( String.IsNullOrEmpty( _telegramHandler.LastSentMessage ), Is.True );
        }
    }
}