namespace Plethora.Logging.log4net.Extensions
{
    /// <summary>
    /// Class used to control the logging when using <see cref="LogConfigurator.BasicConfiguration()"/>.
    /// </summary>
    public static class DynamicLogging
    {
        #region Properties

        /// <summary>
        /// Gets and sets the logging level when using
        /// <see cref="LogConfigurator.BasicConfiguration()"/>.
        /// </summary>
        public static LogLevel Level
        {
            get { return DynamicLevelFilter.GlobalLevel.FromLog4Net(); }
            set { DynamicLevelFilter.GlobalLevel = value.ToLog4Net(); }
        }

        /// <summary>
        /// Gets and sets the pattern to be used when utilising
        /// <see cref="LogConfigurator.BasicConfiguration()"/>.
        /// </summary>
        /// <example>
        ///   <code>
        ///   //Log to the console window
        ///   LogConfigurator.BasicConfiguration(LogLevel.Info);
        ///   DynamicLogging.Pattern = PatternElement.New()
        ///        .Date("HH:mm:ss.fff").Space()
        ///        .Litteral("[").Thread().Litteral("] ")
        ///        .Level(-5).Space()
        ///        .Logger().Space()
        ///        .Litteral("[").Context().Litteral("] - ")
        ///        .Message();
        ///   </code>
        /// </example>
        /// <seealso cref="PatternElement"/>
        public static PatternElement Pattern
        {
            get { return DynamicPatternLayout.GlobalPattern; }
            set { DynamicPatternLayout.GlobalPattern = value; }
        }
        #endregion
    }
}
