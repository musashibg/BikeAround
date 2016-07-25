using BikeAround.Service.Impl.Meta;
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace BikeAround.Service.Impl
{
    public sealed class Logger : ILogTraceEntries, ILogExceptions
    {
        private static Logger _instance;

        private readonly string _logFilePath;

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    Interlocked.CompareExchange(ref _instance, new Logger(), null);
                }
                return _instance;
            }
        }

        public Logger()
        {
            _logFilePath = ConfigurationManager.AppSettings["logFilePath"];
        }

        #region ILogTraceEntries Members

        public void LogTraceEntry(string text)
        {
            File.AppendAllText(_logFilePath, $"[{DateTime.Now:g} TRACE] {text}\r\n");
        }

        #endregion

        #region ILogExceptions Members

        public void LogException(Exception exception)
        {
            File.AppendAllText(_logFilePath, $"[{DateTime.Now:g} EXCEPTION] {exception.GetType().FullName}, \"{exception.Message}\",\r\n{exception.StackTrace}\r\n");
        }

        #endregion
    }
}