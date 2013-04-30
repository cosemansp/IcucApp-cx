using System;
using System.Collections.Generic;

namespace IcucApp.Core.Diagnostics
{
    /// <summary>
    /// Thelog level
    /// </summary>
    public enum LogLevel
    {
        Fatal,
        Error,
        Warn,
        Notice,
        Info,
        Debug,
    }

    /// <summary>
    /// ILog appender
    /// </summary>
    public interface ILogAppender
    {
        void Log(LogLevel level, string name, string text);
        void Log(LogLevel level, string name, object message, Exception exception);
    }

    /// <summary>
    /// A logger interface
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Logs the message as debug
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Logs the message as debug
        /// </summary>
        /// <param name="text">A message.</param>
        void Debug(string text);

        /// <summary>
        /// Logs the message as notice
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void NoticeFormat(string format, params object[] args);

        /// <summary>
        /// Logs the message as notice
        /// </summary>
        /// <param name="message">A  message.</param>
        void Notice(string message);

        /// <summary>
        /// Logs the message as info.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Logs the message as info.
        /// </summary>
        /// <param name="message">A message.</param>
        void Info(string message);

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Logs the message as warning.
        /// </summary>
        /// <param name="message">A formatted message.</param>
        void Warn(string message);

        /// <summary>
        /// Logs the message as error.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Logs the message as fatal.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="message"> </param>
        /// <param name="exception">The exception.</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="message"> </param>
        /// <param name="exception">The exception.</param>
        void Fatal(object message, Exception exception);
    }

    /// <summary>
    /// Used to manage logging.
    /// LogManager.AddAppender(new AndoidLogAppender());
    /// LogManager.AddAppender(new TcpAppender(host, port));
    /// 
    /// </summary>
    public static class LogManager
    {
        public static LogLevel RootLevel { get; set; }
        public static string Filter { get; set; }

        static LogManager()
        {
            RootLevel = LogLevel.Info;
        }

        /// <summary>
        /// Creates an <see cref="ILog"/> for the provided type.
        /// </summary>
        public static ILog GetLogger(Type type)
        {
            return new StandardLogger(type.Name, RootLevel);
        }

        /// <summary>
        /// Creates an <see cref="ILog"/> for the provided type.
        /// </summary>
        public static ILog GetLogger(string name)
        {
            return new StandardLogger(name, RootLevel);
        }


        static internal string SafeFormat(string format, params object[] args)
        {
            try
            {
                return String.Format(format, args);
            }
            catch
            {
                return "FORMAT ERROR: '" + format + "'";
            }
        }

        private class StandardLogger : ILog
        {
            private readonly string _name;
            private readonly LogLevel _rootLevel;

            public StandardLogger(string name, LogLevel rootLevel)
            {
                _name = name;
                _rootLevel = rootLevel;
            }

            private void Log(LogLevel level, string name, string format, params object[] args)
            {
                if (Filter != null)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(name, Filter))
                        return;
                }

                try
                {
                    if (level <= _rootLevel)
                    {
                        var text = format;
                        if (args != null && args.Length > 0)
                            text = SafeFormat(format, args);
                        foreach (var logAppender in AppenderList)
                        {
                            logAppender.Log(level, name, text);
                        }
                    }
                }
                catch
                {
                    // never let a exception escape from here.
                }
            }

            private void Log(LogLevel level, string name, object message, Exception exception)
            {
                try
                {
                    if (level <= _rootLevel)
                    {
                        foreach (var logAppender in AppenderList)
                        {
                            logAppender.Log(level, name, message, exception);
                        }
                    }
                }
                catch
                {
                    // never let a exception escape from here.
                }
            }


            public void DebugFormat(string format, params object[] args)
            {
                Log(LogLevel.Debug, _name, format, args);
            }

            public void Debug(string text)
            {
                Log(LogLevel.Debug, _name, text);
            }

            public void NoticeFormat(string format, params object[] args)
            {
                Log(LogLevel.Notice, _name, format, args);
            }

            public void Notice(string text)
            {
                Log(LogLevel.Notice, _name, text);
            }

            public void InfoFormat(string format, params object[] args)
            {
                Log(LogLevel.Info, _name, format, args);
            }

            public void Info(string text)
            {
                Log(LogLevel.Info, _name, text);
            }

            public void WarnFormat(string format, params object[] args)
            {
                Log(LogLevel.Warn, _name, format, args);
            }

            public void Warn(string text)
            {
                Log(LogLevel.Warn, _name, text);
            }

            public void ErrorFormat(string format, params object[] args)
            {
                Log(LogLevel.Error, _name, format, args);
            }

            public void FatalFormat(string format, params object[] args)
            {
                Log(LogLevel.Fatal, _name, format, args);
            }

            public void Error(object message, Exception exception)
            {
                Log(LogLevel.Error, _name, message, exception);
            }

            public void Fatal(object message, Exception exception)
            {
                Log(LogLevel.Fatal, _name, message, exception);
            }
        }

        private static readonly List<ILogAppender> AppenderList = new List<ILogAppender>();
        public static void AddAppender(ILogAppender appender)
        {
            AppenderList.Add(appender);
        }
    }
}