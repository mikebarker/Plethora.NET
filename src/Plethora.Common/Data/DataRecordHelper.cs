using System;
using System.Data;

namespace Plethora.Data
{
    public static class DataRecordHelper
    {
        public static T GetAs<T>(this IDataRecord dataRecord, int index)
        {
            if (dataRecord.IsDBNull(index))
            {
                if (typeof(T).IsClass || typeof(T).IsNullable())
                    return default(T);
                else
                    throw new InvalidCastException(ResourceProvider.InvalidCast());
            }


            try
            {
                object value = dataRecord.GetValue(index);
                var returnValue = (T)GetAsType(typeof(T), value);
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ResourceProvider.InvalidCast(), ex);
            }
        }


        private static object GetAsType(Type returnType, object value)
        {
            if (returnType.IsInstanceOfType(value))
                return value;

            if (returnType.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(returnType, (string)value);

                object underlyingValue = GetAsType(returnType.GetEnumUnderlyingType(), value);
                object enumValue = Enum.ToObject(returnType, underlyingValue);
                return enumValue;
            }

            if (returnType.IsNullable())
            {
                if (value == null)
                    return null; //Unboxing will take care of type conversion.

                Type underlyingType = Nullable.GetUnderlyingType(returnType);

                //Because of boxing of the underlying type, and the treatment of boxed value by Nullable<T>,
                // no further type conversion needs to be done here.
                object underlyingValue = GetAsType(underlyingType, value);
                return underlyingValue;
            }

            object convertedValue = Convert.ChangeType(value, returnType);
            return convertedValue;
        }

        private static bool IsNullable(this Type type)
        {
            if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return true;

            return false;
        }
    }
}
