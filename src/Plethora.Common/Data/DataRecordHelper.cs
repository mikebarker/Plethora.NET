using System;
using System.Data;

namespace Plethora.Data
{
    public static class DataRecordHelper
    {
        public static T GetAs<T>(this IDataRecord dataRecord, int index)
        {
            Type typeOfT = typeof(T);

            if (dataRecord.IsDBNull(index))
            {
                if (IsNullable(typeOfT))
                    return default(T);
                else
                    throw new InvalidCastException(ResourceProvider.InvalidCast());
            }

            Type baseType = GetBaseValueType(typeOfT);
            object objValue = dataRecord.GetValue(index);

            object baseObjValue;
            try
            {
                //test for enum
                if ((typeOfT.IsEnum) && (objValue is string))
                {
                    baseObjValue = Enum.Parse(typeOfT, (string)objValue);
                }
                else
                {
                    //test if conversion is required
                    baseObjValue = (objValue.GetType() == baseType)
                        ? objValue
                        : Convert.ChangeType(objValue, baseType);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ResourceProvider.InvalidCast(), ex);
            }

            return (T)baseObjValue;
        }


        private static Type GetBaseValueType(Type type)
        {
            if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                var genericArg1 = type.GetGenericArguments()[0];
                return genericArg1;
            }

            if (type.IsEnum)
            {
                return type.GetEnumUnderlyingType();
            }

            return type;
        }

        private static bool IsNullable(Type type)
        {
            if (type.IsClass)
                return true;

            if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return true;

            return false;
        }
    }
}
