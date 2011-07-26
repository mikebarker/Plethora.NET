using System;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to real numeric values.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When binding to the <see cref="RealTextBox.Value"/> property of a <see cref="RealTextBox"/>
    ///   the 'formattingEnabled' must be specified as true.
    ///   <example>
    ///    <code>textBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///   </example>
    ///  </para>
    /// </remarks>
    public class RealTextBox : RealTextBoxBase
    {
    }

    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="RealTextBox"/>.
    /// </summary>
    public class RealTextBoxBase : NumericTextBox<Double>
    {
        #region Overrides of ComparableTextBox<Double>

        protected override Double MinOfT
        {
            get { return Double.MinValue; }
        }

        protected override Double MaxOfT
        {
            get { return Double.MaxValue; }
        }
        #endregion
    }
}
