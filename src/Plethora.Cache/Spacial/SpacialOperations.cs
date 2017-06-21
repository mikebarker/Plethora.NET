using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Plethora.Collections.Sets;

namespace Plethora.Cache.Spacial
{
    public static class SpacialOperations
    {
        #region Subtract

        public static IEnumerable<SpaceRegion<T1>> Subtract<T1>(
            SpaceRegion<T1> regionA,
            SpaceRegion<T1> regionB)
        {
            var list = new List<SpaceRegion<T1>>();

            ISetCore<T1> remainingDim1 = regionA.Dimension1.Subtract(regionB.Dimension1);
            if (remainingDim1.IsEmpty != true)
                list.Add(new SpaceRegion<T1>(remainingDim1));

            return list;
        }

        public static IEnumerable<SpaceRegion<T1, T2>> Subtract<T1, T2>(
            SpaceRegion<T1, T2> regionA,
            SpaceRegion<T1, T2> regionB)
        {
            var list = new List<SpaceRegion<T1, T2>>();

            ISetCore<T1> remainingDim1 = regionA.Dimension1.Subtract(regionB.Dimension1);
            if (remainingDim1.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2>(remainingDim1, regionA.Dimension2));

            ISetCore<T2> remainingDim2 = regionA.Dimension2.Subtract(regionB.Dimension2);
            if (remainingDim2.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2>(regionB.Dimension1, remainingDim2));

            return list;
        }

        public static IEnumerable<SpaceRegion<T1, T2, T3>> Subtract<T1, T2, T3>(
            SpaceRegion<T1, T2, T3> regionA,
            SpaceRegion<T1, T2, T3> regionB)
        {
            var list = new List<SpaceRegion<T1, T2, T3>>();

            ISetCore<T1> remainingDim1 = regionA.Dimension1.Subtract(regionB.Dimension1);
            if (remainingDim1.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3>(remainingDim1, regionA.Dimension2, regionA.Dimension3));

            ISetCore<T2> remainingDim2 = regionA.Dimension2.Subtract(regionB.Dimension2);
            if (remainingDim2.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3>(regionB.Dimension1, remainingDim2, regionA.Dimension3));

            ISetCore<T3> remainingDim3 = regionA.Dimension3.Subtract(regionB.Dimension3);
            if (remainingDim3.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3>(regionB.Dimension1, regionB.Dimension2, remainingDim3));


            return list;
        }

        public static IEnumerable<SpaceRegion<T1, T2, T3, T4>> Subtract<T1, T2, T3, T4>(
            SpaceRegion<T1, T2, T3, T4> regionA,
            SpaceRegion<T1, T2, T3, T4> regionB)
        {
            var list = new List<SpaceRegion<T1, T2, T3, T4>>();

            ISetCore<T1> remainingDim1 = regionA.Dimension1.Subtract(regionB.Dimension1);
            if (remainingDim1.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3, T4>(remainingDim1, regionA.Dimension2, regionA.Dimension3, regionA.Dimension4));

            ISetCore<T2> remainingDim2 = regionA.Dimension2.Subtract(regionB.Dimension2);
            if (remainingDim2.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3, T4>(regionB.Dimension1, remainingDim2, regionA.Dimension3, regionA.Dimension4));

            ISetCore<T3> remainingDim3 = regionA.Dimension3.Subtract(regionB.Dimension3);
            if (remainingDim3.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3, T4>(regionB.Dimension1, regionB.Dimension2, remainingDim3, regionA.Dimension4));

            ISetCore<T4> remainingDim4 = regionA.Dimension4.Subtract(regionB.Dimension4);
            if (remainingDim4.IsEmpty != true)
                list.Add(new SpaceRegion<T1, T2, T3, T4>(regionB.Dimension1, regionB.Dimension2, regionB.Dimension3, remainingDim4));


            return list;
        }

        #endregion

        #region IsPointInRegion

        public static bool IsPointInRegion<T1>([NotNull] Tuple<T1> point, SpaceRegion<T1> region)
        {
            return
                region.Dimension1.Contains(point.Item1);
        }

        public static bool IsPointInRegion<T1, T2>([NotNull] Tuple<T1, T2> point, SpaceRegion<T1, T2> region)
        {
            return
                region.Dimension1.Contains(point.Item1) &&
                region.Dimension2.Contains(point.Item2);
        }

        public static bool IsPointInRegion<T1, T2, T3>([NotNull] Tuple<T1, T2, T3> point, SpaceRegion<T1, T2, T3> region)
        {
            return
                region.Dimension1.Contains(point.Item1) &&
                region.Dimension2.Contains(point.Item2) &&
                region.Dimension3.Contains(point.Item3);
        }

        public static bool IsPointInRegion<T1, T2, T3, T4>([NotNull] Tuple<T1, T2, T3, T4> point, SpaceRegion<T1, T2, T3, T4> region)
        {
            return
                region.Dimension1.Contains(point.Item1) &&
                region.Dimension2.Contains(point.Item2) &&
                region.Dimension3.Contains(point.Item3) &&
                region.Dimension4.Contains(point.Item4);
        }

        #endregion
    }
}
