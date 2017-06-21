using System;
using log4net.Core;
using log4net.Filter;

namespace Plethora.Logging.log4net.Extensions
{
    /// <summary>
    /// Dynamic log4net filter used by <see cref="LogConfigurator.BasicConfiguration()"/>.
    /// </summary>
    public class DynamicLevelFilter : FilterSkeleton
    {
        #region Constructors

        static DynamicLevelFilter()
        {
            GlobalLevel = Level.Info;
        }
        #endregion

        #region FilterSkeleton Overrides

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            //Validation
            if (loggingEvent == null)
                throw new ArgumentNullException(nameof(loggingEvent));

            if ((GlobalLevel != null) && (loggingEvent.Level < GlobalLevel))
            {
                return FilterDecision.Deny;
            }

            return FilterDecision.Neutral;
        }
        #endregion

        #region Properties

        public static Level GlobalLevel { get; set; }
        #endregion
    }
}
