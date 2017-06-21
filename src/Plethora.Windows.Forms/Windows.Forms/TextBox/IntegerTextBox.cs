using System;
using System.ComponentModel;
using Plethora.Windows.Forms.Base;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to integer values.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When binding to the <see cref="IntegerTextBox.Value"/> property of a <see cref="IntegerTextBox"/>
    ///   the 'formattingEnabled' must be specified as true.
    ///   <example>
    ///    <code>textBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///   </example>
    ///  </para>
    /// </remarks>
    [CLSCompliant(false)]
    public class IntegerTextBox : IntegerTextBoxBase
    {
    }
}

namespace Plethora.Windows.Forms.Base
{
    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="IntegerTextBox"/>.
    /// </summary>
    [CLSCompliant(false)]
    [Browsable(false)]
    [ToolboxItem(false)]
    public class IntegerTextBoxBase : NumericTextBox<Int64>
    {
        #region Overrides of ComparableTextBox<Int64>

        protected override Int64 MinOfT
        {
            get { return Int64.MinValue; }
        }

        protected override Int64 MaxOfT
        {
            get { return Int64.MaxValue; }
        }
        #endregion
    }
}
