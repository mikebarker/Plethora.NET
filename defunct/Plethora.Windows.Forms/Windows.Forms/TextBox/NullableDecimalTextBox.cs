using System;
using System.ComponentModel;
using Plethora.Windows.Forms.Base;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to nullable decimal numeric values.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When binding to the <see cref="NullableDecimalTextBox.Value"/> property of a <see cref="NullableDecimalTextBox"/>
    ///   the 'formattingEnabled' must be specified as true.
    ///   <example>
    ///    <code>textBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///   </example>
    ///  </para>
    /// </remarks>
    [CLSCompliant(false)]
    public class NullableDecimalTextBox : NullableDecimalTextBoxBase
    {
    }
}

namespace Plethora.Windows.Forms.Base
{
    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="NullableDecimalTextBox"/>.
    /// </summary>
    [CLSCompliant(false)]
    [Browsable(false)]
    [ToolboxItem(false)]
    public class NullableDecimalTextBoxBase : NullableNumericTextBox<Decimal>
    {
        #region Overrides of ComparableTextBox<Double>

        protected override Decimal? MinOfT
        {
            get { return Decimal.MinValue; }
        }

        protected override Decimal? MaxOfT
        {
            get { return Decimal.MaxValue; }
        }
        #endregion
    }
}
