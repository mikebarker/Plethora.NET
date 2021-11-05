using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Plethora.Context
{
    /// <summary>
    /// Equality comparer for <see cref="ContextInfo"/> objects.
    /// </summary>
    /// <remarks>
    /// Two contexts are considered equal if both the <see cref="ContextInfo.Name"/> and
    /// <see cref="ContextInfo.Data"/> properties match.
    /// </remarks>
    internal sealed class ContextInfoIgnoreRankComparer : IEqualityComparer<ContextInfo>
    {
        #region Constants

        private const int HASHCODE_INITIAL = 0x7D068CCE;
        private const int HASHCODE_ELEMENT = -0x5AAAAAD7;

        #endregion

        #region Implementation of IEqualityComparer<ContextInfo>

        public bool Equals([CanBeNull] ContextInfo x, [CanBeNull] ContextInfo y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null))
                return false;

            if (ReferenceEquals(null, y))
                return false;

            return
                EqualityComparer<string>.Default.Equals(x.Name, y.Name) &&
                EqualityComparer<object>.Default.Equals(x.Data, y.Data);
        }

        public int GetHashCode(ContextInfo context)
        {
            //Validation
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            unchecked
            {
                int num = HASHCODE_INITIAL;
                num = (HASHCODE_ELEMENT * num) + EqualityComparer<string>.Default.GetHashCode(context.Name);
                num = (HASHCODE_ELEMENT * num) + EqualityComparer<object>.Default.GetHashCode(context.Data);
                return num;
            }
        }

        #endregion
    }
}
