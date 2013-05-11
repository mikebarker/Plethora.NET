using System;
using System.ComponentModel;
using Plethora.Windows.Forms.Base;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to decimal values.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When binding to the <see cref="DecimalTextBox.Value"/> property of a <see cref="DecimalTextBox"/>
    ///   the 'formattingEnabled' must be specified as true.
    ///   <example>
    ///    <code>textBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///   </example>
    ///  </para>
    /// </remarks>
    public class DecimalTextBox : DecimalTextBoxBase
    {
    }
}

namespace Plethora.Windows.Forms.Base
{
    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="DecimalTextBox"/>.
    /// </summary>
    [Browsable(false)]
    [ToolboxItem(false)]
    public class DecimalTextBoxBase : NumericTextBox<Decimal>
    {
        #region Overrides of ComparableTextBox<Decimal>

        protected override Decimal MinOfT
        {
            get { return Decimal.MinValue; }
        }

        protected override Decimal MaxOfT
        {
            get { return Decimal.MaxValue; }
        }
        #endregion
    }
}
