using System;
using NLog;

namespace Ammeep.GiftRegister.Web.Domain.Logging
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(string message, Exception exception);
        void LogFatal(string message, Exception exception);
    }

 
    public class LoggingService : ILoggingService
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            if (Logger.IsDebugEnabled)
            {

                Logger.Debug(message);
            }
        }

        public void LogInformation(string message)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info(message);
            }
        }

        public void LogWarning(string message)
        {
            if (Logger.IsWarnEnabled)
            {
                Logger.Warn(message);
            }
        }

        public void LogError(string message)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(message);
            }
        }

        public void LogError(string message, Exception exception)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(message, exception);
            }
        }

        public void LogFatal(string message, Exception exception)
        {
            if (Logger.IsFatalEnabled)
            {
                Logger.Fatal(message, exception);
            }
        }

    }
}