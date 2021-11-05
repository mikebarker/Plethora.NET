using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Plethora.Linq;
using Plethora.Spacial;

namespace Plethora.Cache.Spacial
{
    public abstract class SpacialArgumentBase<TData, TArg, T1> : IArgument<TData, TArg>
        where TArg : SpacialArgumentBase<TData, TArg, T1>
    {
        protected SpacialArgumentBase(
            [NotNull] Func<TData, Tuple<T1>> getPointFromData,
            [NotNull] SpaceRegion<T1> region)
        {
            if (getPointFromData == null)
                throw new ArgumentNullException(nameof(getPointFromData));

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            this.GetPointFromData = getPointFromData;
            this.Region = region;
        }

        [NotNull] public SpaceRegion<T1> Region { get; }
        [NotNull] protected Func<TData, Tuple<T1>> GetPointFromData { get; }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.Region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            var A_minus_B = SpacialOperations.Subtract(this.Region, B.Region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.Region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(r => !r.IsEmpty)
                .Select(r => this.CreateArgWithRegion(r))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Tuple<T1> point = this.GetPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, this.Region);
        }

        [NotNull]
        protected abstract TArg CreateArgWithRegion(SpaceRegion<T1> newRegion);
    }

    public abstract class SpacialArgumentBase<TData, TArg, T1, T2> : IArgument<TData, TArg>
        where TArg : SpacialArgumentBase<TData, TArg, T1, T2>
    {
        protected SpacialArgumentBase(
            [NotNull] Func<TData, Tuple<T1, T2>> getPointFromData,
            [NotNull] SpaceRegion<T1, T2> region)
        {
            if (getPointFromData == null)
                throw new ArgumentNullException(nameof(getPointFromData));

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            this.GetPointFromData = getPointFromData;
            this.Region = region;
        }

        [NotNull] public SpaceRegion<T1, T2> Region { get; }
        [NotNull] protected Func<TData, Tuple<T1, T2>> GetPointFromData { get; }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.Region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            var A_minus_B = SpacialOperations.Subtract(this.Region, B.Region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.Region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(r => !r.IsEmpty)
                .Select(r => this.CreateArgWithRegion(r))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1, T2> point = this.GetPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, this.Region);
        }

        [NotNull]
        protected abstract TArg CreateArgWithRegion(SpaceRegion<T1, T2> newRegion);
    }

    public abstract class SpacialArgumentBase<TData, TArg, T1, T2, T3> : IArgument<TData, TArg>
        where TArg : SpacialArgumentBase<TData, TArg, T1, T2, T3>
    {
        protected SpacialArgumentBase(
            [NotNull] Func<TData, Tuple<T1, T2, T3>> getPointFromData,
            [NotNull] SpaceRegion<T1, T2, T3> region)
        {
            if (getPointFromData == null)
                throw new ArgumentNullException(nameof(getPointFromData));

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            this.GetPointFromData = getPointFromData;
            this.Region = region;
        }

        [NotNull] public SpaceRegion<T1, T2, T3> Region { get; }
        [NotNull] protected Func<TData, Tuple<T1, T2, T3>> GetPointFromData { get; }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.Region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }


            var A_minus_B = SpacialOperations.Subtract(this.Region, B.Region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.Region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(r => !r.IsEmpty)
                .Select(r => this.CreateArgWithRegion(r))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1, T2, T3> point = this.GetPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, this.Region);
        }

        [NotNull]
        protected abstract TArg CreateArgWithRegion(SpaceRegion<T1, T2, T3> newRegion);
    }

    public abstract class SpacialArgumentBase<TData, TArg, T1, T2, T3, T4> : IArgument<TData, TArg>
        where TArg : SpacialArgumentBase<TData, TArg, T1, T2, T3, T4>
    {
        protected SpacialArgumentBase(
            [NotNull] Func<TData, Tuple<T1, T2, T3, T4>> getPointFromData,
            [NotNull] SpaceRegion<T1, T2, T3, T4> region)
        {
            if (getPointFromData == null)
                throw new ArgumentNullException(nameof(getPointFromData));

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            this.GetPointFromData = getPointFromData;
            this.Region = region;
        }

        [NotNull] public SpaceRegion<T1, T2, T3, T4> Region { get; }
        [NotNull] protected Func<TData, Tuple<T1, T2, T3, T4>> GetPointFromData { get; }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.Region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }


            var A_minus_B = SpacialOperations.Subtract(this.Region, B.Region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.Region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(r => !r.IsEmpty)
                .Select(r => this.CreateArgWithRegion(r))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1, T2, T3, T4> point = this.GetPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, this.Region);
        }

        [NotNull]
        protected abstract TArg CreateArgWithRegion(SpaceRegion<T1, T2, T3, T4> newRegion);
    }
}
