using System;

namespace IcucApp.Core.Diagnostics
{
    public class ConsoleAppender : ILogAppender
    {
        public void Log(LogLevel level, string name, string text)
        {
            InternalLog(level, name, text);
        }

        public void Log(LogLevel level, string name, object message, Exception exception)
        {
            InternalLog(level, name, LogManager.SafeFormat("{0}\n\r{1}", message, exception.Message));
        }

        public void InternalLog(LogLevel level, string name, string text)
        {
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("{0} [{1}] {2} - {3}", level, threadId, name, text);
        }
    }
}