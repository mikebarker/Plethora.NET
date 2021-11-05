using System;
using System.Data;

namespace Plethora.Data
{
    public static class DataRecordHelper
    {
        public static T GetAs<T>(this IDataRecord dataRecord, int index)
        {
            if (dataRecord == null)
                throw new ArgumentNullException(nameof(dataRecord));


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
                T returnValue = TypeHelper.As<T>(value);
                return returnValue;
            }
            catch (InvalidCastException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ResourceProvider.InvalidCast(), ex);
            }
        }
    }
}
