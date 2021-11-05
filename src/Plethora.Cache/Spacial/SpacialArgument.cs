using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Plethora.Linq;
using Plethora.Spacial;

namespace Plethora.Cache.Spacial
{
    public class SpacialArgument<TData, T1> : SpacialArgumentBase<TData, SpacialArgument<TData, T1>, T1>
    {
        public SpacialArgument(
            [NotNull] Func<TData, Tuple<T1>> getPointFromData,
            [NotNull] SpaceRegion<T1> region)
            : base(getPointFromData, region)
        {
        }

        [NotNull]
        protected override SpacialArgument<TData,T1> CreateArgWithRegion(SpaceRegion<T1> newRegion)
        {
            return new SpacialArgument<TData, T1>(
                this.GetPointFromData,
                newRegion);
        }
    }

    public class SpacialArgument<TData, T1, T2> : SpacialArgumentBase<TData, SpacialArgument<TData, T1, T2>, T1, T2>
    {
        public SpacialArgument(
            [NotNull] Func<TData, Tuple<T1, T2>> getPointFromData,
            [NotNull] SpaceRegion<T1, T2> region)
            : base(getPointFromData, region)
        {
        }

        [NotNull]
        protected override SpacialArgument<TData, T1, T2> CreateArgWithRegion(SpaceRegion<T1, T2> newRegion)
        {
            return new SpacialArgument<TData, T1, T2>(
                this.GetPointFromData,
                newRegion);
        }
    }

    public class SpacialArgument<TData, T1, T2, T3> : SpacialArgumentBase<TData, SpacialArgument<TData, T1, T2, T3>, T1, T2, T3>
    {
        public SpacialArgument(
            [NotNull] Func<TData, Tuple<T1, T2, T3>> getPointFromData,
            [NotNull] SpaceRegion<T1, T2, T3> region)
            : base(getPointFromData, region)
        {
        }

        [NotNull]
        protected override SpacialArgument<TData, T1, T2, T3> CreateArgWithRegion(SpaceRegion<T1, T2, T3> newRegion)
        {
            return new SpacialArgument<TData, T1, T2, T3>(
                this.GetPointFromData,
                newRegion);
        }
    }

    public class SpacialArgument<TData, T1, T2, T3, T4> : SpacialArgumentBase<TData, SpacialArgument<TData, T1, T2, T3, T4>, T1, T2, T3, T4>
    {
        public SpacialArgument(
            [NotNull] Func<TData, Tuple<T1,T2,T3,T4>> getPointFromData,
            [NotNull] SpaceRegion<T1, T2, T3, T4> region)
            : base(getPointFromData, region)
        {
        }

        [NotNull]
        protected override SpacialArgument<TData, T1, T2, T3, T4> CreateArgWithRegion(SpaceRegion<T1, T2, T3, T4> newRegion)
        {
            return new SpacialArgument<TData, T1, T2, T3, T4>(
                this.GetPointFromData,
                newRegion);
        }
    }
}
