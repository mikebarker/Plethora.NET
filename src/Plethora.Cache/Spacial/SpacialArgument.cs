using System;
using System.Collections.Generic;
using System.Linq;
using Plethora.Linq;

namespace Plethora.Cache.Spacial
{
    public abstract class SpacialArgument<TData, TArg, T1> : IArgument<TData, TArg>
        where TArg : SpacialArgument<TData, TArg, T1>, new()
    {
        private readonly Func<TData, Tuple<T1>> getPointFromData;
        private SpaceRegion<T1> region;

        protected SpacialArgument(
            Func<TData, Tuple<T1>> getPointFromData,
            SpaceRegion<T1> region)
            : this(getPointFromData)
        {
            this.region = region;
        }

        protected SpacialArgument(
            Func<TData, Tuple<T1>> getPointFromData)
        {
            this.getPointFromData = getPointFromData;
        }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            var A_minus_B = SpacialOperations.Subtract(this.region, B.region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(region => !region.IsEmpty)
                .Select(region => this.CreateArgWithRegion(region))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1> point = this.getPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, region);
        }

        private TArg CreateArgWithRegion(SpaceRegion<T1> newRegion)
        {
            var newArg = new TArg();
            newArg.Region = newRegion;
            return newArg;
        }

        protected SpaceRegion<T1> Region
        {
            get { return this.region; }
            private set { this.region = value; }
        }
    }

    public abstract class SpacialArgument<TData, TArg, T1, T2> : IArgument<TData, TArg>
        where TArg : SpacialArgument<TData, TArg, T1, T2>, new()
    {
        private readonly Func<TData, Tuple<T1, T2>> getPointFromData;
        private SpaceRegion<T1, T2> region;

        protected SpacialArgument(
            Func<TData, Tuple<T1, T2>> getPointFromData,
            SpaceRegion<T1, T2> region)
            : this(getPointFromData)
        {
            this.region = region;
        }

        protected SpacialArgument(
            Func<TData, Tuple<T1, T2>> getPointFromData)
        {
            this.getPointFromData = getPointFromData;
        }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            var A_minus_B = SpacialOperations.Subtract(this.region, B.region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(region => !region.IsEmpty)
                .Select(region => this.CreateArgWithRegion(region))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1, T2> point = this.getPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, region);
        }

        private TArg CreateArgWithRegion(SpaceRegion<T1, T2> newRegion)
        {
            var newArg = new TArg();
            newArg.Region = newRegion;
            return newArg;
        }

        protected SpaceRegion<T1, T2> Region
        {
            get { return this.region; }
            private set { this.region = value; }
        }
    }

    public abstract class SpacialArgument<TData, TArg, T1, T2, T3> : IArgument<TData, TArg>
        where TArg : SpacialArgument<TData, TArg, T1, T2, T3>, new()
    {
        private readonly Func<TData, Tuple<T1, T2, T3>> getPointFromData;
        private SpaceRegion<T1, T2, T3> region;

        protected SpacialArgument(
            Func<TData, Tuple<T1, T2, T3>> getPointFromData,
            SpaceRegion<T1, T2, T3> region)
            : this(getPointFromData)
        {
            this.region = region;
        }

        protected SpacialArgument(
            Func<TData, Tuple<T1, T2, T3>> getPointFromData)
        {
            this.getPointFromData = getPointFromData;
        }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }


            var A_minus_B = SpacialOperations.Subtract(this.region, B.region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(region => !region.IsEmpty)
                .Select(region => this.CreateArgWithRegion(region))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1, T2, T3> point = this.getPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, region);
        }

        private TArg CreateArgWithRegion(SpaceRegion<T1, T2, T3> newRegion)
        {
            var newArg = new TArg();
            newArg.Region = newRegion;
            return newArg;
        }

        protected SpaceRegion<T1, T2, T3> Region
        {
            get { return this.region; }
            private set { this.region = value; }
        }
    }

    public abstract class SpacialArgument<TData, TArg, T1, T2, T3, T4> : IArgument<TData, TArg>
        where TArg : SpacialArgument<TData, TArg, T1, T2, T3, T4>, new()
    {
        private readonly Func<TData, Tuple<T1, T2, T3, T4>> getPointFromData;
        private SpaceRegion<T1, T2, T3, T4> region;

        protected SpacialArgument(
            Func<TData, Tuple<T1,T2,T3,T4>> getPointFromData,
            SpaceRegion<T1, T2, T3, T4> region)
            : this(getPointFromData)
        {
            this.region = region;
        }

        protected SpacialArgument(
            Func<TData, Tuple<T1, T2, T3, T4>> getPointFromData)
        {
            this.getPointFromData = getPointFromData;
        }

        public bool IsOverlapped(TArg B, out IEnumerable<TArg> notInB)
        {
            //Short-cut if B is empty
            if (B.region.IsEmpty)
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }


            var A_minus_B = SpacialOperations.Subtract(this.region, B.region).ToListIfRequired();
            if ((A_minus_B.Count == 1) && (A_minus_B[0].Equals(this.region)))
            {
                notInB = ((TArg)this).Singularity();
                return false;
            }

            notInB = A_minus_B
                .Where(region => !region.IsEmpty)
                .Select(region => this.CreateArgWithRegion(region))
                .ToList();

            return true;
        }

        public bool IsDataIncluded(TData data)
        {
            Tuple<T1, T2, T3, T4> point = this.getPointFromData(data);

            return SpacialOperations.IsPointInRegion(point, region);
        }

        private TArg CreateArgWithRegion(SpaceRegion<T1, T2, T3, T4> newRegion)
        {
            var newArg = new TArg();
            newArg.Region = newRegion;
            return newArg;
        }

        protected SpaceRegion<T1, T2, T3, T4> Region
        {
            get { return this.region; }
            private set { this.region = value; }
        }
    }
}
