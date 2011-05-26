using System;
using System.Diagnostics;
using Plethora.Logging.Exceptions;

namespace Plethora.Logging
{
    /// <summary>
    /// Base implementation of the <see cref="ILogger"/> interface.
    /// </summary>
    public abstract class Logger : ILogger
    {
        #region Static Methods

        private static ILoggerProvider loggerProvider;
        public static ILoggerProvider LoggerProvider
        {
            get
            {
                return loggerProvider;
            }
            set
            {
                //Validation
                if (value == null)
                    throw new ArgumentNullException("value");

                loggerProvider = value;
            }
        }

        public static ILogger GetLogger()
        {
            var frame = new StackFrame(1, false);
            return GetLogger(frame.GetMethod().DeclaringType);
        }

        public static ILogger GetLogger(string name)
        {
            return loggerProvider.GetLogger(name);
        }

        public static ILogger GetLogger(Type type)
        {
            return loggerProvider.GetLogger(type);
        }
        #endregion

        #region Implementation of ILogger

        #region Properties

        public virtual bool IsVerboseEnabled
        {
            get { return IsEnabledFor(LogLevel.Verbose); }
        }

        public virtual bool IsDebugEnabled
        {
            get { return IsEnabledFor(LogLevel.Debug); }
        }

        public virtual bool IsInfoEnabled
        {
            get { return IsEnabledFor(LogLevel.Info); }
        }

        public virtual bool IsWarnEnabled
        {
            get { return IsEnabledFor(LogLevel.Warn); }
        }

        public virtual bool IsErrorEnabled
        {
            get { return IsEnabledFor(LogLevel.Error); }
        }

        public virtual bool IsFatalEnabled
        {
            get { return IsEnabledFor(LogLevel.Fatal); }
        }

        #endregion

        #region Verbose

        public virtual void Verbose(string message)
        {
            Log(LogLevel.Verbose, null, () => message);
        }

        public virtual void Verbose(string format, params object[] args)
        {
            Log(LogLevel.Verbose, null, () => string.Format(format, args));
        }

        public virtual void Verbose(Exception exception, string message)
        {
            Log(LogLevel.Verbose, exception, () => message);
        }

        public virtual void Verbose(Exception exception, string format, params object[] args)
        {
            Log(LogLevel.Verbose, exception, () => string.Format(format, args));
        }
        #endregion

        #region Debug

        public virtual void Debug(string message)
        {
            Log(LogLevel.Debug, null, () => message);
        }

        public virtual void Debug(string format, params object[] args)
        {
            Log(LogLevel.Debug, null, () => string.Format(format, args));
        }

        public virtual void Debug(Exception exception, string message)
        {
            Log(LogLevel.Debug, exception, () => message);
        }

        public virtual void Debug(Exception exception, string format, params object[] args)
        {
            Log(LogLevel.Debug, exception, () => string.Format(format, args));
        }
        #endregion

        #region Info

        public virtual void Info(string message)
        {
            Log(LogLevel.Info, null, () => message);
        }

        public virtual void Info(string format, params object[] args)
        {
            Log(LogLevel.Info, null, () => string.Format(format, args));
        }

        public virtual void Info(Exception exception, string message)
        {
            Log(LogLevel.Info, exception, () => message);
        }

        public virtual void Info(Exception exception, string format, params object[] args)
        {
            Log(LogLevel.Info, exception, () => string.Format(format, args));
        }
        #endregion

        #region Warn

        public virtual void Warn(string message)
        {
            Log(LogLevel.Warn, null, () => message);
        }

        public virtual void Warn(string format, params object[] args)
        {
            Log(LogLevel.Warn, null, () => string.Format(format, args));
        }

        public virtual void Warn(Exception exception, string message)
        {
            Log(LogLevel.Warn, exception, () => message);
        }

        public virtual void Warn(Exception exception, string format, params object[] args)
        {
            Log(LogLevel.Warn, exception, () => string.Format(format, args));
        }
        #endregion

        #region Error

        public virtual void Error(string message)
        {
            Log(LogLevel.Error, null, () => message);
        }

        public virtual void Error(string format, params object[] args)
        {
            Log(LogLevel.Error, null, () => string.Format(format, args));
        }

        public virtual void Error(Exception exception, string message)
        {
            Log(LogLevel.Error, exception, () => message);
        }

        public virtual void Error(Exception exception, string format, params object[] args)
        {
            Log(LogLevel.Error, exception, () => string.Format(format, args));
        }
        #endregion

        #region Fatal

        public virtual void Fatal(string message)
        {
            Log(LogLevel.Fatal, null, () => message);
        }

        public virtual void Fatal(string format, params object[] args)
        {
            Log(LogLevel.Fatal, null, () => string.Format(format, args));
        }

        public virtual void Fatal(Exception exception, string message)
        {
            Log(LogLevel.Fatal, exception, () => message);
        }

        public virtual void Fatal(Exception exception, string format, params object[] args)
        {
            Log(LogLevel.Fatal, exception, () => string.Format(format, args));
        }
        #endregion

        #endregion

        #region Abstract Members

        protected abstract bool IsEnabledFor(LogLevel logLevel);
        protected abstract void ForceLog(LogLevel logLevel, Exception exception, string message);
        #endregion

        #region Non-Public Methods

        protected virtual void Log(LogLevel logLevel, Exception exception, Func<string> messageProvider)
        {
            //If the exception is logged then skip
            if (IsLogged(exception))
                return;

            //Test if logging is enabled
            if (!IsEnabledFor(logLevel))
                return;

            //Log the message
            string message = messageProvider();
            ForceLog(logLevel, exception, message);

            //Mark the exception as logged (if required)
            MarkAsLogged(exception);
        }

        private static bool IsLogged(Exception exception)
        {
            var isLoggedException = exception as IsLoggedException;
            if (isLoggedException == null)
                return false;

            return isLoggedException.IsLogged;
        }

        private static void MarkAsLogged(Exception exception)
        {
            var isLoggedException = exception as IsLoggedException;
            if (isLoggedException == null)
                return;

            isLoggedException.IsLogged = true;
        }
        #endregion
    }
}
