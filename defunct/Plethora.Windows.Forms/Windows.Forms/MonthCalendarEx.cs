using System;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.Drawing;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// MonthCalendar with additional DateDoubleClick functionality
    /// </summary>
    [ToolboxBitmapEx(typeof(Properties.Resources), "Calendar")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class MonthCalendarEx : MonthCalendar
    {
        #region Fields

        private static readonly long doubleClickTicks = TimeSpan.TicksPerMillisecond * SystemInformation.DoubleClickTime;

        private long prevClickTimeStamp = DateTime.MinValue.Ticks;
        private DateTime prevSelectionRangeStart = DateTime.MinValue;
        private DateTime prevSelectionRangeEnd = DateTime.MinValue;
        #endregion

        #region DateDoubleClick Event

        private static readonly object DateDoubleClick_EventKey = new object();

        /// <summary>
        /// Raises when a date is double-clicked.
        /// </summary>
        [Category(ControlAttributes.Category.Action)]
        [Description("Occurs when a date is double-clicked.")]
        public event EventHandler DateDoubleClick
        {
            add { base.Events.AddHandler(DateDoubleClick_EventKey, value); }
            remove { base.Events.RemoveHandler(DateDoubleClick_EventKey, value); }
        }

        /// <summary>
        /// Raises the <see cref="DateDoubleClick"/> event.
        /// </summary>
        protected virtual void OnDateDoubleClick(EventArgs e)
        {
            var handler = base.Events[DateDoubleClick_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region MonthCalendar Overrides

        //Hack in double click logic.
        protected override void OnDateSelected(DateRangeEventArgs drevent)
        {
            base.OnDateSelected(drevent);

            if ((drevent.Start == prevSelectionRangeStart) &&
                (drevent.End == prevSelectionRangeEnd))
            {
                if ((DateTime.Now.Ticks - prevClickTimeStamp) < doubleClickTicks)
                {
                    this.OnDateDoubleClick(EventArgs.Empty);
                }
            }

            this.prevSelectionRangeStart = drevent.Start;
            this.prevSelectionRangeEnd = drevent.End;
            this.prevClickTimeStamp = DateTime.Now.Ticks;
        }
        #endregion
    }
}
