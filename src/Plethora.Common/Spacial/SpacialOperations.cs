using System;
using System.Collections.Generic;
using Plethora.Collections.Sets;

namespace Plethora.Spacial
{
    public static class SpacialOperations
    {
        #region Subtract

        private static IEnumerable<TSpaceRegion> SubtractInternal<TSpaceRegion>(
            TSpaceRegion regionA,
            TSpaceRegion regionB,
            Func<ISetCore[], TSpaceRegion> ctorFunc)
            where TSpaceRegion : SpaceRegion
        {
            var list = new List<TSpaceRegion>();

            var remainingDimensions = (ISetCore[])regionA.Dimensions.Clone();

            for (int i = 0; i< regionA.Dimensions.Length; i++)
            {
                if (i > 0)
                {
                    remainingDimensions[i - 1] = regionB.Dimensions[i - 1];
                }

                var remainingDimensionSets = regionA.Dimensions[i].SubtractMulti(regionB.Dimensions[i]);
                foreach (var remainingDim in remainingDimensionSets)
                {
                    if (remainingDim.IsEmpty != true)
                    {
                        remainingDimensions[i] = remainingDim;
                        list.Add(ctorFunc(remainingDimensions));
                    }
                }
            }

            return list;
        }

        public static IEnumerable<SpaceRegion> Subtract(
            SpaceRegion regionA,
            SpaceRegion regionB)
        {
            return SubtractInternal<SpaceRegion>(
                regionA,
                regionB,
                d => new SpaceRegion(d));
        }

        public static IEnumerable<SpaceRegion<T1>> Subtract<T1>(
            SpaceRegion<T1> regionA,
            SpaceRegion<T1> regionB)
        {
            return SubtractInternal<SpaceRegion<T1>>(
                regionA,
                regionB,
                d =>
                {
                    return new SpaceRegion<T1>(
                        (ISetCore<T1>)d[0]);
                });
        }

        public static IEnumerable<SpaceRegion<T1, T2>> Subtract<T1, T2>(
            SpaceRegion<T1, T2> regionA,
            SpaceRegion<T1, T2> regionB)
        {
            return SubtractInternal<SpaceRegion<T1, T2>>(
                regionA,
                regionB,
                d =>
                {
                    return new SpaceRegion<T1, T2>(
                        (ISetCore<T1>)d[0], 
                        (ISetCore<T2>)d[1]);
                });
        }

        public static IEnumerable<SpaceRegion<T1, T2, T3>> Subtract<T1, T2, T3>(
            SpaceRegion<T1, T2, T3> regionA,
            SpaceRegion<T1, T2, T3> regionB)
        {
            return SubtractInternal<SpaceRegion<T1, T2, T3>>(
                regionA,
                regionB,
                d =>
                {
                    return new SpaceRegion<T1, T2, T3>(
                        (ISetCore<T1>)d[0],
                        (ISetCore<T2>)d[1],
                        (ISetCore<T3>)d[2]);
                });
        }

        public static IEnumerable<SpaceRegion<T1, T2, T3, T4>> Subtract<T1, T2, T3, T4>(
            SpaceRegion<T1, T2, T3, T4> regionA,
            SpaceRegion<T1, T2, T3, T4> regionB)
        {
            return SubtractInternal<SpaceRegion<T1, T2, T3, T4>>(
                regionA,
                regionB,
                d =>
                {
                    return new SpaceRegion<T1, T2, T3, T4>(
                        (ISetCore<T1>)d[0],
                        (ISetCore<T2>)d[1],
                        (ISetCore<T3>)d[2],
                        (ISetCore<T4>)d[3]);
                });
        }

        #endregion

        #region IsPointInRegion

        public static bool IsPointInRegion<T1>(Tuple<T1> point, SpaceRegion<T1> region)
        {
            return
                region.Dimension1.Contains(point.Item1);
        }

        public static bool IsPointInRegion<T1, T2>(Tuple<T1, T2> point, SpaceRegion<T1, T2> region)
        {
            return
                region.Dimension1.Contains(point.Item1) &&
                region.Dimension2.Contains(point.Item2);
        }

        public static bool IsPointInRegion<T1, T2, T3>(Tuple<T1, T2, T3> point, SpaceRegion<T1, T2, T3> region)
        {
            return
                region.Dimension1.Contains(point.Item1) &&
                region.Dimension2.Contains(point.Item2) &&
                region.Dimension3.Contains(point.Item3);
        }

        public static bool IsPointInRegion<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> point, SpaceRegion<T1, T2, T3, T4> region)
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
