using System;

namespace Plethora.Logging
{
    /// <summary>
    /// Interface for providing logging functionality.
    /// </summary>
    public interface ILogger
    {
        #region Properties

        bool IsVerboseEnabled { get; }

        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }
        #endregion

        #region Verbose

        void Verbose(string message);

        void Verbose(string format, params object[] args);

        void Verbose(Exception exception, string message);

        void Verbose(Exception exception, string format, params object[] args);
        #endregion

        #region Debug

        void Debug(string message);

        void Debug(string format, params object[] args);

        void Debug(Exception exception, string message);

        void Debug(Exception exception, string format, params object[] args);
        #endregion

        #region Info

        void Info(string message);

        void Info(string format, params object[] args);

        void Info(Exception exception, string message);

        void Info(Exception exception, string format, params object[] args);
        #endregion

        #region Warn

        void Warn(string message);

        void Warn(string format, params object[] args);

        void Warn(Exception exception, string message);

        void Warn(Exception exception, string format, params object[] args);
        #endregion

        #region Error

        void Error(string message);

        void Error(string format, params object[] args);

        void Error(Exception exception, string message);

        void Error(Exception exception, string format, params object[] args);
        #endregion

        #region Fatal

        void Fatal(string message);

        void Fatal(string format, params object[] args);

        void Fatal(Exception exception, string message);

        void Fatal(Exception exception, string format, params object[] args);
        #endregion
    }
}
