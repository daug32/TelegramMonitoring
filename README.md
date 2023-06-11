# TelegramMonitoring

Provides a background service that forwards a message from monitoring to Telegram chat.
This requires monitorings to have a GET api endpoint that returns a string.

## Configurating
### Configure scheduler
You can edit scheduling delay in [appsettings.json](https://github.com/daug32/TelegramMonitoring/blob/main/MonitoringScheduler/appsettings.json): 
```JSON
{
  "Scheduling": {
    "Delay": "1:00:00"
  }
}
```

### Configure projects
Add your settings.
Project model is [here](https://github.com/daug32/TelegramMonitoring/blob/main/Monitoring.Core/Configurations/ProjectConfiguration.cs)
```JSON
{
  "ProjectConfigurations": [
    {
      "ProjectName": "Test project name",
      "NotifyIfMonitoringReturnedEmptyMessage": false,
      "TelegramBotConfiguration": {
        "ApiKey": "0000000000:AAAAAAAAAAAAAAA_AAAAAAA_AAAAAAAAAAA"
      },
      "TelegramChatConfiguration": {
        "ChatId": -1000000000000
      },
      "MonitoringConfiguration": {
        "AuthenticationToken": "",
        "AuthenticationTokenHeader": "",
        "Url": "test.com/get-monitoring-message"
      }
    }
  ]
}
```
