using System;
using System.ComponentModel;

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
    public class NullableDecimalTextBox : NullableDecimalTextBoxBase
    {
    }

    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="NullableDecimalTextBox"/>.
    /// </summary>
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
