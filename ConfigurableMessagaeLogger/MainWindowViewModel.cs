using Amazon.Auth.AccessControlPolicy;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConfigurableMessagaeLogger
{
    //enums that will be used to retrieve or list type of event elements
    public enum LogTypes
    {
        Error,
        Warning,
        Information
    }
    public class MainWindowViewModel : ObservableObject
    {
        ILoggerService _loggerService;
        DateTime _SelectedDate;
        ICommand _showLogCommand;
        ICommand _SubmitLogCommand;
        string _Message;
        string _Application;
        
        ObservableCollection<LogMessage> _logMessage;
        LogMessage _LogMessageItem;

        public string Message 
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged(Message);
            }
        }
        public string Application 
        {
            get 
            { 
                return _Application; 
            }
            set
            {
                _Application = value;
                OnPropertyChanged(nameof(Application));
            }
        } 

        public ObservableCollection<LogTypes> ListLogTypes
        {
            get
            {
                return new ObservableCollection<LogTypes> { LogTypes.Error, LogTypes.Warning, LogTypes.Information };
            }                     
        }
        public ObservableCollection<LogMessage> LogMessages
        {
            get
            {
                if (_logMessage == null)
                    _logMessage = new ObservableCollection<LogMessage>();
                return _logMessage;
            }
            set
            {
                _logMessage = value;
                OnPropertyChanged(nameof(LogMessages));
            }
        }

        public LogTypes SelectedType { get; set; }
        public ILoggerService LoggerService 
        {
            get
            {
                return _loggerService;
            }
            set 
            {
                _loggerService = value;
                OnPropertyChanged(nameof(_loggerService));
            }
        }
        public ICommand ShowLogCommand
        {
            get
            {
                if (_showLogCommand == null)
                    _showLogCommand = new RelayCommand(_FilterLogsByType);
                return _showLogCommand;
            }
        }
        public ICommand SubmitLogCommand
        {
            get
            {
                if (_SubmitLogCommand == null)
                    _SubmitLogCommand = new RelayCommand(_SubmitLog);
                return _SubmitLogCommand;
            }

        }

        //ILoggerService is passed from MainWindow.cs where datacontext is set to the actual MainWindowViewModel in order to separate code from ui (MVVM)
        //ILoggerService make use of dependency injection that has been declared and instantiated in app.cs
        public MainWindowViewModel(ILoggerService loggerService)
        {
            _loggerService = loggerService;
            LogMessages = _InitLogs();
   

        }

        private ObservableCollection<LogMessage> _InitLogs()
        {
            try
            {
                LoggerService.LogMessages = JsonConvert.DeserializeObject<ObservableCollection<LogMessage>>(_readLogFiles()) ?? throw new JsonException();
                return LoggerService.LogMessages ?? throw new ArgumentException();
            }catch(JsonException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new ObservableCollection<LogMessage>(); //To avoid getting an error rteturn an empty list

        }

        private string _readLogFiles()
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(@"C:\Users\BECCO1SAR\source\repos\ConfigurableMessagaeLogger\ConfigurableMessagaeLogger\generated_logs.json")) 
                { 
                    return streamReader.ReadToEnd();
                
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }

        }
        public LogMessage LogMessageItem 
        {
            get
            {
                return _LogMessageItem;
            }
            set
            {
                _LogMessageItem = value;
                OnPropertyChanged(nameof(_LogMessageItem));
            }
        }

        public DateTime SelectedDate 
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                _SelectedDate = value;
                //OnPropertyChanged(nameof(_SelectedDate));
                _FilterLogsByDate();
            }
        }

        private void _SubmitLog(object obj)
        {
            try
            {
                
                if (SelectedType == LogTypes.Error)
                    LoggerService.LogError(Message, Application);
                else if (SelectedType == LogTypes.Warning)
                    LoggerService.LogWarning(Message, Application);
                else if (SelectedType == LogTypes.Information)
                    LoggerService.LogInfo(Message, Application);

                LogMessages = new ObservableCollection<LogMessage>(LoggerService.LogMessages);
            }catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _ClearFields();
            }
     
        }

        private void _ClearFields()
        {
            Message = string.Empty;
            Application = string.Empty;
        }

        private void _FilterLogsByType(object obj)
        {
            LogMessages = new ObservableCollection<LogMessage>(LoggerService.LogMessages.Where(x => x.Type.ToString() == obj.ToString()));
        }
        private void _FilterLogsByDate()
        {
            LogMessages = new ObservableCollection<LogMessage>(LoggerService.LogMessages.Where(x => x.TimeStamp.Date == SelectedDate.Date));
        }
    }
}
