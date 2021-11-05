using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Plethora.Xaml.Wpf.Converters
{
    internal enum ArithmeticOperator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Modulus
    }

    internal static class ArithmeticConverterHelper
    {
        public static object ExecuteOperator(ArithmeticOperator arithmeticOperator, object lvalue, object rvalue)
        {
            MethodInfo method = GetOperatorMethod(arithmeticOperator, lvalue.GetType(), rvalue.GetType());
            if (method == null)
                throw new InvalidOperationException();

            object result = method.Invoke(null, new[] { lvalue, rvalue });
            return result;
        }

        /// <summary>
        /// Converts the type as is done implicitly by the C# compiler for arithmetic operations between two types.
        /// </summary>
        /// <remarks>
        /// Both returned reference values will be of the same type.
        /// The returned values will be one of:
        ///   <list type="bullet">
        ///     <item><description>Int32</description></item>
        ///     <item><description>UInt32</description></item>
        ///     <item><description>Int64</description></item>
        ///     <item><description>UInt64</description></item>
        ///     <item><description>Single</description></item>
        ///     <item><description>Double</description></item>
        ///     <item><description>Decimal</description></item>
        ///   </list>
        /// </remarks>
        public static void ConvertForOperation(ref object lvalue, ref object rvalue)
        {
            if (lvalue is byte)
            {
                if (rvalue is byte)
                {
                    lvalue = (int)(byte)lvalue;
                    rvalue = (int)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    lvalue = (int)(byte)lvalue;
                    rvalue = (int)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    lvalue = (int)(byte)lvalue;
                    rvalue = (int)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    lvalue = (int)(byte)lvalue;
                    rvalue = (int)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    lvalue = (int)(byte)lvalue;
                }
                else if (rvalue is uint)
                {
                    lvalue = (uint)(byte)lvalue;
                }
                else if (rvalue is long)
                {
                    lvalue = (long)(byte)lvalue;
                }
                else if (rvalue is ulong)
                {
                    lvalue = (ulong)(byte)lvalue;
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(byte)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(byte)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(byte)lvalue;
                }
            }
            else if (lvalue is sbyte)
            {
                if (rvalue is byte)
                {
                    lvalue = (int)(sbyte)lvalue;
                    rvalue = (int)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    lvalue = (int)(sbyte)lvalue;
                    rvalue = (int)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    lvalue = (int)(sbyte)lvalue;
                    rvalue = (int)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    lvalue = (int)(sbyte)lvalue;
                    rvalue = (int)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    lvalue = (int)(sbyte)lvalue;
                }
                else if (rvalue is uint)
                {
                    lvalue = (long)(sbyte)lvalue;
                    rvalue = (long)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    lvalue = (long)(sbyte)lvalue;
                }
                else if (rvalue is ulong)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(sbyte)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(sbyte)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(sbyte)lvalue;
                }
            }
            else if (lvalue is short)
            {
                if (rvalue is byte)
                {
                    lvalue = (int)(short)lvalue;
                    rvalue = (int)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    lvalue = (int)(short)lvalue;
                    rvalue = (int)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    lvalue = (int)(short)lvalue;
                    rvalue = (int)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    lvalue = (int)(short)lvalue;
                    rvalue = (int)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    lvalue = (int)(short)lvalue;
                }
                else if (rvalue is uint)
                {
                    lvalue = (long)(short)lvalue;
                    rvalue = (long)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    lvalue = (long)(short)lvalue;
                }
                else if (rvalue is ulong)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(short)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(short)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(short)lvalue;
                }
            }
            else if (lvalue is ushort)
            {
                if (rvalue is byte)
                {
                    lvalue = (int)(ushort)lvalue;
                    rvalue = (int)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    lvalue = (int)(ushort)lvalue;
                    rvalue = (int)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    lvalue = (int)(ushort)lvalue;
                    rvalue = (int)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    lvalue = (int)(ushort)lvalue;
                    rvalue = (int)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    lvalue = (int)(ushort)lvalue;
                }
                else if (rvalue is uint)
                {
                    lvalue = (uint)(ushort)lvalue;
                }
                else if (rvalue is long)
                {
                    lvalue = (long)(ushort)lvalue;
                }
                else if (rvalue is ulong)
                {
                    lvalue = (ulong)(ushort)lvalue;
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(ushort)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(ushort)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(ushort)lvalue;
                }
            }
            else if (lvalue is int)
            {
                if (rvalue is byte)
                {
                    rvalue = (int)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    rvalue = (int)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    rvalue = (int)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    rvalue = (int)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                }
                else if (rvalue is uint)
                {
                    lvalue = (long)(int)lvalue;
                    rvalue = (long)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    lvalue = (long)(int)lvalue;
                }
                else if (rvalue is ulong)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(int)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(int)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(int)lvalue;
                }
            }
            else if (lvalue is uint)
            {
                if (rvalue is byte)
                {
                    rvalue = (uint)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    lvalue = (long)(uint)lvalue;
                    rvalue = (long)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    lvalue = (long)(uint)lvalue;
                    rvalue = (long)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    rvalue = (uint)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    lvalue = (long)(uint)lvalue;
                    rvalue = (long)(int)rvalue;
                }
                else if (rvalue is uint)
                {
                }
                else if (rvalue is long)
                {
                    lvalue = (long)(uint)lvalue;
                }
                else if (rvalue is ulong)
                {
                    lvalue = (ulong)(uint)lvalue;
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(uint)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(uint)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(uint)lvalue;
                }
            }
            else if (lvalue is long)
            {
                if (rvalue is byte)
                {
                    rvalue = (long)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    rvalue = (long)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    rvalue = (long)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    rvalue = (long)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    rvalue = (long)(int)rvalue;
                }
                else if (rvalue is uint)
                {
                    rvalue = (long)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                }
                else if (rvalue is ulong)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(long)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(long)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(long)lvalue;
                }
            }
            else if (lvalue is ulong)
            {
                if (rvalue is byte)
                {
                    rvalue = (ulong)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is short)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is ushort)
                {
                    rvalue = (ulong)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is uint)
                {
                    rvalue = (ulong)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is ulong)
                {
                }
                else if (rvalue is float)
                {
                    lvalue = (float)(ulong)lvalue;
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(ulong)lvalue;
                }
                else if (rvalue is decimal)
                {
                    lvalue = (decimal)(ulong)lvalue;
                }
            }
            else if (lvalue is float)
            {
                if (rvalue is byte)
                {
                    rvalue = (float)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    rvalue = (float)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    rvalue = (float)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    rvalue = (float)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    rvalue = (float)(int)rvalue;
                }
                else if (rvalue is uint)
                {
                    rvalue = (float)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    rvalue = (float)(long)rvalue;
                }
                else if (rvalue is ulong)
                {
                    rvalue = (float)(ulong)rvalue;
                }
                else if (rvalue is float)
                {
                }
                else if (rvalue is double)
                {
                    lvalue = (double)(float)lvalue;
                }
                else if (rvalue is decimal)
                {
                    throw new InvalidCastException();
                }
            }
            else if (lvalue is double)
            {
                if (rvalue is byte)
                {
                    rvalue = (double)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    rvalue = (double)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    rvalue = (double)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    rvalue = (double)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    rvalue = (double)(int)rvalue;
                }
                else if (rvalue is uint)
                {
                    rvalue = (double)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    rvalue = (double)(long)rvalue;
                }
                else if (rvalue is ulong)
                {
                    rvalue = (double)(ulong)rvalue;
                }
                else if (rvalue is float)
                {
                    rvalue = (double)(float)rvalue;
                }
                else if (rvalue is double)
                {
                }
                else if (rvalue is decimal)
                {
                    throw new InvalidCastException();
                }
            }
            else if (lvalue is decimal)
            {
                if (rvalue is byte)
                {
                    rvalue = (decimal)(byte)rvalue;
                }
                else if (rvalue is sbyte)
                {
                    rvalue = (decimal)(sbyte)rvalue;
                }
                else if (rvalue is short)
                {
                    rvalue = (decimal)(short)rvalue;
                }
                else if (rvalue is ushort)
                {
                    rvalue = (decimal)(ushort)rvalue;
                }
                else if (rvalue is int)
                {
                    rvalue = (decimal)(int)rvalue;
                }
                else if (rvalue is uint)
                {
                    rvalue = (decimal)(uint)rvalue;
                }
                else if (rvalue is long)
                {
                    rvalue = (decimal)(long)rvalue;
                }
                else if (rvalue is ulong)
                {
                    rvalue = (decimal)(ulong)rvalue;
                }
                else if (rvalue is float)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is double)
                {
                    throw new InvalidCastException();
                }
                else if (rvalue is decimal)
                {
                }
            }
        }

        private static MethodInfo GetOperatorMethod(ArithmeticOperator arithmeticOperator, Type lvalueType, Type rvalueType)
        {
            string methodName = GetOperatorMethodName(arithmeticOperator);
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
            Type[] argumentTypes = {lvalueType, rvalueType};

            MethodInfo operatorMethod = lvalueType.GetMethod(
                methodName,
                bindingFlags,
                null,
                argumentTypes,
                null);

            if (operatorMethod == null)
            {
                operatorMethod = rvalueType.GetMethod(
                    methodName,
                    bindingFlags,
                    null,
                    argumentTypes,
                    null);
            }

            return operatorMethod;
        }

        private static string GetOperatorMethodName(ArithmeticOperator arithmeticOperator)
        {
            switch (arithmeticOperator)
            {
                case ArithmeticOperator.Addition:
                    return "op_Addition";

                case ArithmeticOperator.Subtraction:
                    return "op_Subtraction";

                case ArithmeticOperator.Multiplication:
                    return "op_Multiply";

                case ArithmeticOperator.Division:
                    return "op_Division";

                case ArithmeticOperator.Modulus:
                    return "op_Modulus";
            }

            throw new InvalidEnumArgumentException(nameof(arithmeticOperator));
        }


        public static object ConvertType(object value, Type targetType, IFormatProvider provider)
        {
            if (targetType.IsInstanceOfType(value))
                return value;

            if ((value == null) && (!targetType.IsValueType))
                return null;

            if ((targetType.IsGenericType) && (targetType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                if (value == null)
                    return null; // Unboxing will take care of type conversion.

                Type underlyingType = Nullable.GetUnderlyingType(targetType);

                //Because of boxing of the underlying type, and the treatment of boxed value by Nullable<T>,
                // no further type conversion needs to be done here.
                object underlyingValue = ConvertType(value, underlyingType, provider);
                return underlyingValue;
            }

            if (targetType.IsEnum)
            {
                Type underlyingType = targetType.GetEnumUnderlyingType();
                object underlyingValue = ConvertType(value, underlyingType, provider);
                object enumValue = Enum.ToObject(targetType, underlyingValue);
                return enumValue;
            }

            if (value is IConvertible)
            {
                object result = Convert.ChangeType(value, targetType, provider);
                return result;
            }

            MethodInfo castMethod = FindCastMethod(value, targetType);
            if (castMethod != null)
            {
                object result = castMethod.Invoke(null, new[] {value});
                return result;
            }

            throw new InvalidCastException();
        }


        private static MethodInfo FindCastMethod(object value, Type toType)
        {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;

            IEnumerable<MethodInfo> methods = toType.GetMethods(bindingFlags);
            if (value != null)
            {
                methods = methods.Concat(value.GetType().GetMethods(bindingFlags));
            }

            foreach (MethodInfo method in methods)
            {
                if ((method.IsSpecialName) &&
                    ((method.Name == "op_Implicit") || (method.Name == "op_Explicit")) &&
                    (toType.IsAssignableFrom(method.ReturnType)))
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 1)
                    {
                        Type parameterType = parameters[0].ParameterType;
                        if (((value != null) && (parameterType.IsInstanceOfType(value))) ||
                            ((value == null) && (!parameterType.IsValueType)))
                        {
                            return method;
                        }
                    }
                }
            }

            return null;
        }

    }
}
