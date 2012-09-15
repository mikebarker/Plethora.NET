using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Cache.Spacial
{
    public abstract class SpacialArgument<TSpacialArgument, TData, TKey1> : IArgument<TData, TSpacialArgument>
        where TSpacialArgument : SpacialArgument<TSpacialArgument, TData, TKey1>
    {
        #region Fields

        private readonly SpacialRegion<TKey1> keyRegion;
        #endregion

        #region Constructors

        protected SpacialArgument(SpacialRegion<TKey1> keyRegion)
        {
            this.keyRegion = keyRegion;
        }
        #endregion

        #region Implementation of IArgument<T,SpacialArgument<T>>

        /// <summary>
        /// Gets a value indicating whether two arguments overlap in the key-space which they represent.
        /// </summary>
        public bool IsOverlapped(
            TSpacialArgument B,
            out IEnumerable<TSpacialArgument> notInB)
        {
            if (!keyRegion.DoesRegionOverlap(B.keyRegion))
            {
                notInB = null;
                return false;
            }

            notInB = keyRegion.Subtract(B.keyRegion)
                .Select(r => CreateNew(r))
                .ToList();
            return true;
        }

        /// <summary>
        /// A filtering function which returns a flag indicating whether data is represented by this
        /// argument instance.
        /// </summary>
        public bool IsDataIncluded(TData data)
        {
            var point = GetKeyPointFromData(data);
            return this.keyRegion.IsPointInRegion(point);
        }

        #endregion

        #region Abstract Members

        protected abstract Tuple<TKey1> GetKeyPointFromData(TData data);
        protected abstract TSpacialArgument CreateNew(SpacialRegion<TKey1> region);
        #endregion
    }

    public abstract class SpacialArgument<TSpacialArgument, TData, TKey1, TKey2> : IArgument<TData, TSpacialArgument>
        where TSpacialArgument : SpacialArgument<TSpacialArgument, TData, TKey1, TKey2>
    {
        #region Fields

        private readonly SpacialRegion<TKey1, TKey2> keyRegion;
        #endregion

        #region Constructors

        protected SpacialArgument(SpacialRegion<TKey1, TKey2> keyRegion)
        {
            this.keyRegion = keyRegion;
        }
        #endregion

        #region Implementation of IArgument<T,SpacialArgument<T>>

        /// <summary>
        /// Gets a value indicating whether two arguments overlap in the key-space which they represent.
        /// </summary>
        public bool IsOverlapped(
            TSpacialArgument B,
            out IEnumerable<TSpacialArgument> notInB)
        {
            if (!keyRegion.DoesRegionOverlap(B.keyRegion))
            {
                notInB = null;
                return false;
            }

            notInB = keyRegion.Subtract(B.keyRegion)
                .Select(r => CreateNew(r))
                .ToList();
            return true;
        }

        /// <summary>
        /// A filtering function which returns a flag indicating whether data is represented by this
        /// argument instance.
        /// </summary>
        public bool IsDataIncluded(TData data)
        {
            var point = GetKeyPointFromData(data);
            return this.keyRegion.IsPointInRegion(point);
        }

        #endregion

        #region Abstract Members

        protected abstract Tuple<TKey1, TKey2> GetKeyPointFromData(TData data);
        protected abstract TSpacialArgument CreateNew(SpacialRegion<TKey1, TKey2> region);
        #endregion
    }

    public abstract class SpacialArgument<TSpacialArgument, TData, TKey1, TKey2, TKey3> : IArgument<TData, TSpacialArgument>
        where TSpacialArgument : SpacialArgument<TSpacialArgument, TData, TKey1, TKey2, TKey3>
    {
        #region Fields

        private readonly SpacialRegion<TKey1, TKey2, TKey3> keyRegion;
        #endregion

        #region Constructors

        protected SpacialArgument(SpacialRegion<TKey1, TKey2, TKey3> keyRegion)
        {
            this.keyRegion = keyRegion;
        }
        #endregion

        #region Implementation of IArgument<T,SpacialArgument<T>>

        /// <summary>
        /// Gets a value indicating whether two arguments overlap in the key-space which they represent.
        /// </summary>
        public bool IsOverlapped(
            TSpacialArgument B,
            out IEnumerable<TSpacialArgument> notInB)
        {
            if (!keyRegion.DoesRegionOverlap(B.keyRegion))
            {
                notInB = null;
                return false;
            }

            notInB = keyRegion.Subtract(B.keyRegion)
                .Select(r => CreateNew(r))
                .ToList();
            return true;
        }

        /// <summary>
        /// A filtering function which returns a flag indicating whether data is represented by this
        /// argument instance.
        /// </summary>
        public bool IsDataIncluded(TData data)
        {
            var point = GetKeyPointFromData(data);
            return this.keyRegion.IsPointInRegion(point);
        }

        #endregion

        #region Abstract Members

        protected abstract Tuple<TKey1, TKey2, TKey3> GetKeyPointFromData(TData data);
        protected abstract TSpacialArgument CreateNew(SpacialRegion<TKey1, TKey2, TKey3> region);
        #endregion
    }

    public abstract class SpacialArgument<TSpacialArgument, TData, TKey1, TKey2, TKey3, TKey4> : IArgument<TData, TSpacialArgument>
        where TSpacialArgument : SpacialArgument<TSpacialArgument, TData, TKey1, TKey2, TKey3, TKey4>
    {
        #region Fields

        private readonly SpacialRegion<TKey1, TKey2, TKey3, TKey4> keyRegion;
        #endregion

        #region Constructors

        protected SpacialArgument(SpacialRegion<TKey1, TKey2, TKey3, TKey4> keyRegion)
        {
            this.keyRegion = keyRegion;
        }
        #endregion

        #region Implementation of IArgument<T,SpacialArgument<T>>

        /// <summary>
        /// Gets a value indicating whether two arguments overlap in the key-space which they represent.
        /// </summary>
        public bool IsOverlapped(
            TSpacialArgument B,
            out IEnumerable<TSpacialArgument> notInB)
        {
            if (!keyRegion.DoesRegionOverlap(B.keyRegion))
            {
                notInB = null;
                return false;
            }

            notInB = keyRegion.Subtract(B.keyRegion)
                .Select(r => CreateNew(r))
                .ToList();
            return true;
        }

        /// <summary>
        /// A filtering function which returns a flag indicating whether data is represented by this
        /// argument instance.
        /// </summary>
        public bool IsDataIncluded(TData data)
        {
            var point = GetKeyPointFromData(data);
            return this.keyRegion.IsPointInRegion(point);
        }

        #endregion

        #region Abstract Members

        protected abstract Tuple<TKey1, TKey2, TKey3, TKey4> GetKeyPointFromData(TData data);
        protected abstract TSpacialArgument CreateNew(SpacialRegion<TKey1, TKey2, TKey3, TKey4> region);
        #endregion
    }
}
