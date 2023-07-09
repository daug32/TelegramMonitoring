# TelegramMonitoring

Provides a background service that forwards a message from monitoring to Telegram chat.
This requires monitorings to have a GET api endpoint that returns a string.

## Configurating
Add your settings. To see all available settings check the [Project model](https://github.com/daug32/TelegramMonitoring/blob/main/Monitoring.Core/Configurations/ProjectConfiguration.cs).
```JSON
{
  "ProjectConfigurations": [
    {
      "ProjectName": "Test project name",
      "NotifyIfMonitoringReturnedEmptyMessage": false,
      "Delay": "24:00:00",
      "TelegramBotConfiguration": {
        "ApiKey": "0000000000:AAAAAAAAAAAAAAA_AAAAAAA_AAAAAAAAAAA"
      },
      "TelegramChatConfiguration": {
        "ChatId": -1000000000000
      },
      "AppMonitoringConfiguration": {
        "Url": "test.com/get-monitoring-message"
      }
    }
  ]
}
```
