using System;
using log4net.Core;

namespace Plethora.Logging.log4net
{
    internal static class LogLevelHelper
    {
        public static Level ToLog4Net(this LogLevel logLevel)
        {
            if (logLevel <= 0)
                throw new InvalidCastException(ResourceProvider.InvalidCast());

            if (logLevel >= LogLevel.Off)
                return Level.Off;
            if (logLevel >= LogLevel.Fatal)
                return Level.Fatal;
            if (logLevel >= LogLevel.Error)
                return Level.Error;
            if (logLevel >= LogLevel.Warn)
                return Level.Warn;
            if (logLevel >= LogLevel.Info)
                return Level.Info;
            if (logLevel >= LogLevel.Debug)
                return Level.Debug;

            return Level.Verbose;
        }

        public static LogLevel FromLog4Net(this Level level)
        {
            if (level >= Level.Off)
                return LogLevel.Off;

            if (level >= Level.Fatal)
                return LogLevel.Fatal;

            if (level >= Level.Error)
                return LogLevel.Error;

            if (level >= Level.Warn)
                return LogLevel.Warn;

            if (level >= Level.Info)
                return LogLevel.Info;

            if (level >= Level.Debug)
                return LogLevel.Debug;

            if (level >= Level.Verbose)
                return LogLevel.Verbose;

            throw new InvalidCastException(ResourceProvider.InvalidCast());
        }
    }
}
