using System.Collections;
using System.Collections.Generic;

namespace Plethora
{
    /// <summary>
    /// Class for generating reliable hash codes.
    /// </summary>
    /// <remarks>
    /// The algorithms for generating these hash codes is taken from
    /// the .NET implementation of anonymous classes.
    /// </remarks>
    public static class HashCodeHelper
    {
        #region Constants

        private const int HASHCODE_INITIAL = 0x7D068CCE;
        private const int HASHCODE_ELEMENT = -0x5AAAAAD7;
        #endregion

        #region GetHashCode

        public static int GetHashCode<T>(T? item)
        {
            if (item is null)
                return 0;

            return EqualityComparer<T>.Default.GetHashCode(item);
        }

        public static int GetHashCode<T1, T2>(
            T1 item1, T2 item2)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT * num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT * num) + GetHashCode(item2);
                return num;
            }
        }

        public static int GetHashCode<T1, T2, T3>(
            T1 item1, T2 item2, T3 item3)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item2);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item3);
                return num;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(
            T1 item1, T2 item2, T3 item3, T4 item4)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item2);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item3);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item4);
                return num;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(
            T1 item1, T2 item2, T3 item3, T4 item4,
            T5 item5)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item2);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item3);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item4);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item5);
                return num;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(
            T1 item1, T2 item2, T3 item3, T4 item4,
            T5 item5, T6 item6)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item2);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item3);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item4);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item5);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item6);
                return num;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(
            T1 item1, T2 item2, T3 item3, T4 item4,
            T5 item5, T6 item6, T7 item7)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item2);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item3);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item4);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item5);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item6);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item7);
                return num;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8>(
            T1 item1, T2 item2, T3 item3, T4 item4,
            T5 item5, T6 item6, T7 item7, T8 item8)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item1);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item2);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item3);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item4);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item5);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item6);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item7);
                num = (HASHCODE_ELEMENT*num) + GetHashCode(item8);
                return num;
            }
        }
        #endregion

        #region GetEnumerableHashCode

        public static int GetEnumerableHashCode(IEnumerable enumerable)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                foreach (var item in enumerable)
                {
                    num = (HASHCODE_ELEMENT * num) + item.GetHashCode();
                }
                return num;
            }
        }
        #endregion

        #region Generate

        public readonly struct HashCodeElement
        {
            private readonly int num;

            internal HashCodeElement(int num)
            {
                this.num = num;
            }

            public readonly int Num => this.num;

            public readonly HashCodeElement Then<T>(T value)
            {
                unchecked
                {
                    int nextNum = (HASHCODE_ELEMENT * this.num) + HashCodeHelper.GetHashCode(value);
                    return new HashCodeElement(nextNum);
                }
            }

            public static implicit operator int(HashCodeElement element) => element.num;
        }

        public static HashCodeElement Generate<T>(T value)
        {
            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT * num) + HashCodeHelper.GetHashCode(value);
                return new HashCodeElement(num);
            }
        }

        #endregion
    }
}
