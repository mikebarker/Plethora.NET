using System;

namespace Plethora
{
    /// <summary>
    /// Helper class which contains various methods for working with types, and data-type conversions.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Converts the value provided to type specified.
        /// </summary>
        /// <typeparam name="T">The return type required.</typeparam>
        /// <param name="value"></param>
        /// <param name="value">The value to be converted.</param>
        /// <returns>
        /// The value of <paramref name="value"/> as type <paramref name="returnType"/>.
        /// </returns>
        /// <see cref="AsType(Type, object)"/>
        public static T? As<T>(this object? value)
        {
            return (T?)AsType(value, typeof(T));
        }

        /// <summary>
        /// Converts the value provided to type specified.
        /// </summary>
        /// <param name="returnType">The return type required.</param>
        /// <param name="value">The value to be converted.</param>
        /// <returns>
        /// The value of <paramref name="value"/> as type <paramref name="returnType"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="returnType"/> is null.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     This conversion is not supported.
        ///     -or-
        ///     <paramref name="value"/> is null and <paramref name="returnType"/> is a value type other than <see cref="Nullable{T}"/>.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     <paramref name="value"/> represents a number that is out of the range of <paramref name="returnType"/>.
        /// </exception>
        public static object? AsType(this object? value, Type returnType)
        {
            ArgumentNullException.ThrowIfNull(returnType);


            if (returnType.IsInstanceOfType(value))
                return value;

            try
            {
                if (returnType.IsEnum)
                {
                    if (value is string strEnumValue)
                        return Enum.Parse(returnType, strEnumValue);

                    object? underlyingValue = AsType(value, returnType.GetEnumUnderlyingType());
                    object enumValue = Enum.ToObject(returnType, underlyingValue!);
                    return enumValue;
                }

                if ((returnType == typeof(Guid)) && (value is string strGuidValue))
                {
                    Guid guidValue = Guid.Parse(strGuidValue);
                    return guidValue;
                }

                if (returnType.IsNullable())
                {
                    if (value is null)
                        return null; //Unboxing will take care of type conversion.

                    Type? underlyingType = Nullable.GetUnderlyingType(returnType);

                    //Because of boxing of the underlying type, and the treatment of boxed value by Nullable<T>,
                    // no further type conversion needs to be done here.
                    object? underlyingValue = AsType(value, underlyingType!);
                    return underlyingValue;
                }

                object? convertedValue = Convert.ChangeType(value, returnType);
                return convertedValue;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidCastException(ResourceProvider.InvalidCast(), ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidCastException(ResourceProvider.InvalidCast(), ex);
            }
        }

        /// <summary>
        /// Gets a flag indicating whether the specified type is of the generic type <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="type">The type to be tested for being a generic <see cref="Nullable{T}"/> type.</param>
        /// <returns>
        /// True if the <paramref name="type"/> is a generic <see cref="Nullable{T}"/> type; otherwise False.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public static bool IsNullable(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);


            if (type.IsClass)
                return true;

            if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return true;

            return false;
        }
    }
}
