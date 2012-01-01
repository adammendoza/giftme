using System;
using System.Reflection;
using log4net;

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

        private static readonly ILog _log4NetLoggger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);   

        public void LogDebug(string message)
        {
            if (_log4NetLoggger.IsDebugEnabled)
            {
                _log4NetLoggger.Debug(message);
            }
        }

        public void LogInformation(string message)
        {
            if (_log4NetLoggger.IsInfoEnabled)
            {
                _log4NetLoggger.Info(message);
            }
        }

        public void LogWarning(string message)
        {
            if (_log4NetLoggger.IsWarnEnabled)
            {
                _log4NetLoggger.Warn(message);
            }
        }

        public void LogError(string message)
        {
            if (_log4NetLoggger.IsErrorEnabled)
            {
                _log4NetLoggger.Error(message);
            }
        }

        public void LogError(string message, Exception exception)
        {
            if (_log4NetLoggger.IsErrorEnabled)
            {
                _log4NetLoggger.Error(message, exception);
            }
        }

        public void LogFatal(string message, Exception exception)
        {
            if (_log4NetLoggger.IsFatalEnabled)
            {
                _log4NetLoggger.Fatal(message, exception);
            }
        }

    }
}