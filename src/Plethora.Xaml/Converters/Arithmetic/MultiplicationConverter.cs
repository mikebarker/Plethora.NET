using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to multiply two values.
    /// </summary>
    /// <remarks>
    /// Returns (value[0] * value[1])
    /// </remarks>
    public class MultiplicationConverter : ArithmeticConverterBase
    {
        protected override object Operate(object lvalue, object rvalue)
        {
            object result;

            if ((lvalue is int) && (rvalue is int))
            {
                result = ((int)lvalue) * ((int)rvalue);
            }
            else if ((lvalue is uint) && (rvalue is uint))
            {
                result = ((uint)lvalue) * ((uint)rvalue);
            }
            else if ((lvalue is long) && (rvalue is long))
            {
                result = ((long)lvalue) * ((long)rvalue);
            }
            else if ((lvalue is ulong) && (rvalue is ulong))
            {
                result = ((ulong)lvalue) * ((ulong)rvalue);
            }
            else if ((lvalue is float) && (rvalue is float))
            {
                result = ((float)lvalue) * ((float)rvalue);
            }
            else if ((lvalue is double) && (rvalue is double))
            {
                result = ((double)lvalue) * ((double)rvalue);
            }
            else if ((lvalue is decimal) && (rvalue is decimal))
            {
                result = ((decimal)lvalue) * ((decimal)rvalue);
            }
            else
            {
                result = ArithmeticConverterHelper.ExecuteOperator(
                    ArithmeticOperator.Multiplication,
                    lvalue,
                    rvalue);
            }

            return result;
        }
    }
}
