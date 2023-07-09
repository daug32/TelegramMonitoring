namespace Monitoring.Core.Implementation.Utils
{
    public static class MessageBuilder
    {
        public static string BuildMessage( string projectName, string message )
        {
            return $"Application: \"{projectName}\".\nMessage: {message}";
        }

        public static string BuildRequestErrorMessage( string projectName, string exceptionMessage )
        {
            return
                $"Application: \"{projectName}\". Couldn't get message from application.\nMessage: {exceptionMessage}";
        }
    }
}