using NLog;
using System;

namespace Start.Infrastructure
{
    public class Logger
    {
        public ILogger Nlogger { get; internal set; }

        public static Logger Create(string loggerName)
        {
            return new Logger
            {
                Nlogger = LogManager.GetLogger(loggerName)
            };
        }
        
        public void Error(Exception ex)
        {
            Nlogger.Error(ex, ex.Message);
        }

        public void Info(string message)
        {
            Nlogger.Info(message);
        }

        public string PrepareMessage(string message)
        {
            return $"{message}|{Nlogger.Name}";
        }
    }
}
