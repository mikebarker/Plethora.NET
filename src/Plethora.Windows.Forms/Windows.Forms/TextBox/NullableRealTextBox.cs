using System;
using System.ComponentModel;
using Plethora.Windows.Forms.Base;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to nullable real numeric values.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When binding to the <see cref="NullableRealTextBox.Value"/> property of a <see cref="NullableRealTextBox"/>
    ///   the 'formattingEnabled' must be specified as true.
    ///   <example>
    ///    <code>textBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///   </example>
    ///  </para>
    /// </remarks>
    public class NullableRealTextBox : NullableRealTextBoxBase
    {
    }
}

namespace Plethora.Windows.Forms.Base
{
    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="NullableRealTextBox"/>.
    /// </summary>
    [Browsable(false)]
    [ToolboxItem(false)]
    public class NullableRealTextBoxBase : NullableNumericTextBox<Double>
    {
        #region Overrides of ComparableTextBox<Double>

        protected override Double? MinOfT
        {
            get { return Double.MinValue; }
        }

        protected override Double? MaxOfT
        {
            get { return Double.MaxValue; }
        }
        #endregion
    }
}
