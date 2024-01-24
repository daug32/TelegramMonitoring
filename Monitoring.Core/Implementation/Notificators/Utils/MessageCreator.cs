namespace Monitoring.Core.Implementation.Notificators.Utils
{
    public static class MessageCreator
    {
        public static string Create( string projectName, string message )
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