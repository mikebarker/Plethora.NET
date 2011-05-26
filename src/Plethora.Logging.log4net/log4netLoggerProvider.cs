using System;
using log4net;

namespace Plethora.Logging.log4net
{
    public class log4netLoggerProvider : ILoggerProvider
    {
        #region Implementation of ILoggerProvider

        public ILogger GetLogger(string name)
        {
            ILog log = LogManager.GetLogger(name);
            return GetLogger(log);
        }

        public ILogger GetLogger(Type type)
        {
            ILog log = LogManager.GetLogger(type);
            return GetLogger(log);
        }
        #endregion

        #region Private Methods

        private static ILogger GetLogger(ILog log)
        {
            return new log4netLogger(log);
        }
        #endregion
    }
}
