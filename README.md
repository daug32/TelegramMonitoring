# TelegramMonitoring

Provides a background service that forwards a message from monitoring to Telegram chat.
This requires monitorings to have a GET api endpoint that returns a string.

## Configurating
### Configure scheduler 
You can edit scheduling delay in [appsettings.json](https://github.com/daug32/TelegramMonitoring/blob/main/Monitoring.BackgroundService/appsettings.json): 
```JSON
{
  "Scheduling": {
    "DelayInMinutes": 60
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
      "TelegramBotConfiguration": {
        "ApiKey": ""
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
