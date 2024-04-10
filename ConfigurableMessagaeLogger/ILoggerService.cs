using System.Collections.ObjectModel;

namespace ConfigurableMessagaeLogger
{
    public interface ILoggerService
    {
        void LogInfo(string message, string application);
        void LogError(string message, string application);
        void LogWarning(string message, string application);
        void ClearLog();
        ObservableCollection<LogMessage> LogMessages { get; set; }

    }
}