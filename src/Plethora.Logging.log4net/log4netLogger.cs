using System;
using log4net;
using log4net.Core;

namespace Plethora.Logging.log4net
{
    public class log4netLogger : Logger
    {
        #region Fields

        private readonly ILog innerLog;
        #endregion

        #region Constructor

        public log4netLogger(ILog log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.innerLog = log;
        }
        #endregion

        #region Overrides of Logger

        protected override void ForceLog(LogLevel logLevel, Exception exception, string message)
        {
            Level level = logLevel.ToLog4Net();
            innerLog.Logger.Log(null, level, message, exception);
        }

        protected override bool IsEnabledFor(LogLevel logLevel)
        {
            return innerLog.Logger.IsEnabledFor(logLevel.ToLog4Net());
        }
        #endregion
    }
}
