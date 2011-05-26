using System.IO;
using log4net.Config;
using Plethora.Logging.log4net.Extensions;
using Plethora.Logging.log4net.Properties;

namespace Plethora.Logging.log4net
{
    /// <summary>
    /// Configures log4net.
    /// </summary>
    /// <seealso cref="DynamicLogging"/>
    public static class LogConfigurator
    {
        /// <summary>
        /// Configures log4net with a basic configuration, which logs to the windows Console,
        /// with the log level set to 'Info'.
        /// </summary>
        /// <remarks>
        /// The configuration returned can be dynamically adjusted using the
        /// <see cref="DynamicLogging"/> class.
        /// </remarks>
        public static void BasicConfiguration()
        {
            BasicConfiguration(LogLevel.Info);
        }

        /// <summary>
        /// Configures log4net with a basic configuration, which logs to the windows Console.
        /// </summary>
        /// <param name="logLevel">The level of logging required.</param>
        /// <remarks>
        /// The configuration returned can be dynamically adjusted using the
        /// <see cref="DynamicLogging"/> class.
        /// </remarks>
        public static void BasicConfiguration(LogLevel logLevel)
        {
            DynamicLogging.Level = logLevel;

            using (Stream stream = new MemoryStream(Resources.BasicConfiguration))
            {
                XmlConfigurator.Configure(stream);
            }
        }
    }
}
