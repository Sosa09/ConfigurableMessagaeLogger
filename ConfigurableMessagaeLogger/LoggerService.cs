using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace ConfigurableMessagaeLogger
{

    //inheriting ILoggerService Interface
    public class LoggerService : ObservableObject, ILoggerService
    {
 
        private static object lockObject = new object();
        ObservableCollection<LogMessage> _logMessages;


        public ObservableCollection<LogMessage> LogMessages 
        {
            get
            {
                if (_logMessages == null)
                    _logMessages = new ObservableCollection<LogMessage>();
                return _logMessages;
            }
            set
            {
                _logMessages = value;
                OnPropertyChanged(nameof(LogMessages));
             
            }
        }

        public LoggerService()
        {

        }


        public void ClearLog()
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message, string application = "")
        {
            _InsertLog(message, LogTypes.Information, application);
        }

        public void LogError(string message, string application = "")
        {
            _InsertLog(message, LogTypes.Error, application);
        }

        public void LogWarning(string message, string application = "")
        {
            _InsertLog(message, LogTypes.Warning, application);
        }

        private void _InsertLog(string message, LogTypes logType, string application = "")
        {
            Random r = new Random();
            LogMessages.Add(
                new LogMessage { 
                    Id=r.Next(), 
                    Message = message, 
                    TimeStamp = DateTime.Now, 
                    Type = logType, 
                    Application = application 
                });
            //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, LogMessages[LogMessages.Count - 1 ])); CHECK HOW TO SHOW MESSAGE BASED ON COLLECTIONCHANGED
         
        }

        //TODO: Add filters here
    }
}
