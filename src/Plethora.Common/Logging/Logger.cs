using System;
using System.Diagnostics;

namespace Plethora.Logging
{
    /// <summary>
    /// Base implementation of the <see cref="ILogger"/> interface.
    /// </summary>
    public abstract class Logger : ILogger
    {
        #region Static Methods

        private static ILoggerProvider? loggerProvider;
        public static ILoggerProvider LoggerProvider
        {
            get
            {
                return loggerProvider!;
            }
            set
            {
                //Validation
                ArgumentNullException.ThrowIfNull(value);

                loggerProvider = value;
            }
        }

        public static ILogger GetLogger(string name)
        {
            return loggerProvider!.GetLogger(name);
        }

        public static ILogger GetLogger(Type type)
        {
            return loggerProvider!.GetLogger(type);
        }
        #endregion

        #region Implementation of ILogger

        #region Properties

        public virtual bool IsVerboseEnabled
        {
            get { return this.IsEnabledFor(LogLevel.Verbose); }
        }

        public virtual bool IsDebugEnabled
        {
            get { return this.IsEnabledFor(LogLevel.Debug); }
        }

        public virtual bool IsInfoEnabled
        {
            get { return this.IsEnabledFor(LogLevel.Info); }
        }

        public virtual bool IsWarnEnabled
        {
            get { return this.IsEnabledFor(LogLevel.Warn); }
        }

        public virtual bool IsErrorEnabled
        {
            get { return this.IsEnabledFor(LogLevel.Error); }
        }

        public virtual bool IsFatalEnabled
        {
            get { return this.IsEnabledFor(LogLevel.Fatal); }
        }

        #endregion

        #region Verbose

        public virtual void Verbose(string message)
        {
            this.Log(LogLevel.Verbose, null, () => message);
        }

        public virtual void Verbose(string format, params object[] args)
        {
            this.Log(LogLevel.Verbose, null, () => string.Format(format, args));
        }

        public virtual void Verbose(Exception exception, string message)
        {
            this.Log(LogLevel.Verbose, exception, () => message);
        }

        public virtual void Verbose(Exception exception, string format, params object[] args)
        {
            this.Log(LogLevel.Verbose, exception, () => string.Format(format, args));
        }
        #endregion

        #region Debug

        public virtual void Debug(string message)
        {
            this.Log(LogLevel.Debug, null, () => message);
        }

        public virtual void Debug(string format, params object[] args)
        {
            this.Log(LogLevel.Debug, null, () => string.Format(format, args));
        }

        public virtual void Debug(Exception exception, string message)
        {
            this.Log(LogLevel.Debug, exception, () => message);
        }

        public virtual void Debug(Exception exception, string format, params object[] args)
        {
            this.Log(LogLevel.Debug, exception, () => string.Format(format, args));
        }
        #endregion

        #region Info

        public virtual void Info(string message)
        {
            this.Log(LogLevel.Info, null, () => message);
        }

        public virtual void Info(string format, params object[] args)
        {
            this.Log(LogLevel.Info, null, () => string.Format(format, args));
        }

        public virtual void Info(Exception exception, string message)
        {
            this.Log(LogLevel.Info, exception, () => message);
        }

        public virtual void Info(Exception exception, string format, params object[] args)
        {
            this.Log(LogLevel.Info, exception, () => string.Format(format, args));
        }
        #endregion

        #region Warn

        public virtual void Warn(string message)
        {
            this.Log(LogLevel.Warn, null, () => message);
        }

        public virtual void Warn(string format, params object[] args)
        {
            this.Log(LogLevel.Warn, null, () => string.Format(format, args));
        }

        public virtual void Warn(Exception exception, string message)
        {
            this.Log(LogLevel.Warn, exception, () => message);
        }

        public virtual void Warn(Exception exception, string format, params object[] args)
        {
            this.Log(LogLevel.Warn, exception, () => string.Format(format, args));
        }
        #endregion

        #region Error

        public virtual void Error(string message)
        {
            this.Log(LogLevel.Error, null, () => message);
        }

        public virtual void Error(string format, params object[] args)
        {
            this.Log(LogLevel.Error, null, () => string.Format(format, args));
        }

        public virtual void Error(Exception exception, string message)
        {
            this.Log(LogLevel.Error, exception, () => message);
        }

        public virtual void Error(Exception exception, string format, params object[] args)
        {
            this.Log(LogLevel.Error, exception, () => string.Format(format, args));
        }
        #endregion

        #region Fatal

        public virtual void Fatal(string message)
        {
            this.Log(LogLevel.Fatal, null, () => message);
        }

        public virtual void Fatal(string format, params object[] args)
        {
            this.Log(LogLevel.Fatal, null, () => string.Format(format, args));
        }

        public virtual void Fatal(Exception exception, string message)
        {
            this.Log(LogLevel.Fatal, exception, () => message);
        }

        public virtual void Fatal(Exception exception, string format, params object[] args)
        {
            this.Log(LogLevel.Fatal, exception, () => string.Format(format, args));
        }
        #endregion

        #endregion

        #region Abstract Members

        protected abstract bool IsEnabledFor(LogLevel logLevel);
        protected abstract void ForceLog(LogLevel logLevel, Exception? exception, string message);
        #endregion

        #region Non-Public Methods

        protected virtual void Log(LogLevel logLevel, Exception? exception, Func<string> messageProvider)
        {
            //If the exception is logged then skip
            if (IsLogged(exception))
                return;

            //Test if logging is enabled
            if (!this.IsEnabledFor(logLevel))
                return;

            //Log the message
            string message = messageProvider();
            this.ForceLog(logLevel, exception, message);

            //Mark the exception as logged (if required)
            MarkAsLogged(exception);
        }

        private static bool IsLogged(Exception? exception)
        {
            if (exception is not IsLoggedException isLoggedException)
                return false;

            return isLoggedException.IsLogged;
        }

        private static void MarkAsLogged(Exception? exception)
        {
            if (exception is not IsLoggedException isLoggedException)
                return;

            isLoggedException.IsLogged = true;
        }
        #endregion
    }
}
