using System;
using JetBrains.Annotations;

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

        [StringFormatMethod("message")]
        void Verbose(string message);

        [StringFormatMethod("message")]
        void Verbose(string format, params object[] args);

        [StringFormatMethod("message")]
        void Verbose(Exception exception, string message);

        [StringFormatMethod("message")]
        void Verbose(Exception exception, string format, params object[] args);
        #endregion

        #region Debug

        [StringFormatMethod("message")]
        void Debug(string message);

        [StringFormatMethod("format")]
        void Debug(string format, params object[] args);

        [StringFormatMethod("message")]
        void Debug(Exception exception, string message);

        [StringFormatMethod("format")]
        void Debug(Exception exception, string format, params object[] args);
        #endregion

        #region Info

        [StringFormatMethod("message")]
        void Info(string message);

        [StringFormatMethod("format")]
        void Info(string format, params object[] args);

        [StringFormatMethod("message")]
        void Info(Exception exception, string message);

        [StringFormatMethod("format")]
        void Info(Exception exception, string format, params object[] args);
        #endregion

        #region Warn

        [StringFormatMethod("message")]
        void Warn(string message);

        [StringFormatMethod("format")]
        void Warn(string format, params object[] args);

        [StringFormatMethod("message")]
        void Warn(Exception exception, string message);

        [StringFormatMethod("format")]
        void Warn(Exception exception, string format, params object[] args);
        #endregion

        #region Error

        [StringFormatMethod("message")]
        void Error(string message);

        [StringFormatMethod("format")]
        void Error(string format, params object[] args);

        [StringFormatMethod("message")]
        void Error(Exception exception, string message);

        [StringFormatMethod("format")]
        void Error(Exception exception, string format, params object[] args);
        #endregion

        #region Fatal

        [StringFormatMethod("message")]
        void Fatal(string message);

        [StringFormatMethod("format")]
        void Fatal(string format, params object[] args);

        [StringFormatMethod("message")]
        void Fatal(Exception exception, string message);

        [StringFormatMethod("format")]
        void Fatal(Exception exception, string format, params object[] args);
        #endregion
    }
}
