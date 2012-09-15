using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Plethora.Cache.Spacial
{
    public struct SpacialRegion<T1>
    {
        #region Fields

        private readonly SpacialDefinition<T1> spacialDefinition;

        private readonly Range<T1> dim1;
        #endregion

        #region Constructors

        public SpacialRegion(
            SpacialDefinition<T1> spacialDefinition,
            Range<T1> dim1)
        {
            //Validation
            if (spacialDefinition == null)
                throw new ArgumentNullException("spacialDefinition");


            this.spacialDefinition = spacialDefinition;

            this.dim1 = dim1;
        }
        #endregion

        #region Public Methods

        [Pure]
        public bool DoesRegionOverlap(SpacialRegion<T1> other)
        {
            return
                this.spacialDefinition.Dimension1.DoDimensionsOverlap(this.dim1, other.dim1);
        }

        [Pure]
        public bool IsPointInRegion(Tuple<T1> point)
        {
            return
                spacialDefinition.Dimension1.IsPointInDimension(point.Item1, this.dim1);
        }

        [Pure]
        public IEnumerable<SpacialRegion<T1>> Subtract(SpacialRegion<T1> other)
        {
            //Ensure the SpacialRegions pertain to the same key space.
            if (this.spacialDefinition == other.spacialDefinition)
                throw new ArgumentException( /* TODO: error message */ );


            SpacialRegion<T1> region;
            List<SpacialRegion<T1>> regions = new List<SpacialRegion<T1>>(2 * 4);

            // D1
            if (TryCreateRegion(
                out region,
                this.dim1.Min, other.dim1.Min, true, false))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Max, this.dim1.Max, false, true))
            {
                regions.Add(region);
            }


            regions.TrimExcess();
            return regions;
        }
        #endregion

        #region Private Members

        private bool TryCreateRegion(
            out SpacialRegion<T1> region,
            T1 dim1Min, T1 dim1Max, bool dim1MinIncluded, bool dim1MaxIncluded)
        {
            //Validate dimension 1
            if (spacialDefinition.Dimension1.Compare(dim1Min, dim1Max) > 0)
            {
                region = default(SpacialRegion<T1>);
                return false;
            }


            region = new SpacialRegion<T1>(
                this.spacialDefinition,
                new Range<T1>(dim1Min, dim1Max, dim1MinIncluded, dim1MaxIncluded));

            return true;
        }
        #endregion
    }

    public struct SpacialRegion<T1, T2>
    {
        #region Fields

        private readonly SpacialDefinition<T1, T2> spacialDefinition;

        private readonly Range<T1> dim1;
        private readonly Range<T2> dim2;
        #endregion

        #region Constructors

        public SpacialRegion(
            SpacialDefinition<T1, T2> spacialDefinition,
            Range<T1> dim1,
            Range<T2> dim2)
        {
            //Validation
            if (spacialDefinition == null)
                throw new ArgumentNullException("spacialDefinition");


            this.spacialDefinition = spacialDefinition;

            this.dim1 = dim1;
            this.dim2 = dim2;
        }
        #endregion

        #region Public Methods

        [Pure]
        public bool DoesRegionOverlap(SpacialRegion<T1, T2> other)
        {
            return
                this.spacialDefinition.Dimension1.DoDimensionsOverlap(this.dim1, other.dim1) &&
                this.spacialDefinition.Dimension2.DoDimensionsOverlap(this.dim2, other.dim2);
        }

        [Pure]
        public bool IsPointInRegion(Tuple<T1, T2> point)
        {
            return
                spacialDefinition.Dimension1.IsPointInDimension(point.Item1, this.dim1) &&
                spacialDefinition.Dimension2.IsPointInDimension(point.Item2, this.dim2);
        }

        [Pure]
        public IEnumerable<SpacialRegion<T1, T2>> Subtract(SpacialRegion<T1, T2> other)
        {
            //Ensure the SpacialRegions pertain to the same key space.
            if (this.spacialDefinition == other.spacialDefinition)
                throw new ArgumentException( /* TODO: error message */ );


            SpacialRegion<T1, T2> region;
            List<SpacialRegion<T1, T2>> regions = new List<SpacialRegion<T1, T2>>(2 * 4);

            // D1
            if (TryCreateRegion(
                out region,
                this.dim1.Min, other.dim1.Min, true, false,
                this.dim2.Min, this.dim2.Max, true, true))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Max, this.dim1.Max, false, true,
                this.dim2.Min, this.dim2.Max, true, true))
            {
                regions.Add(region);
            }

            // D2
            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                this.dim2.Min, other.dim2.Min, true, false))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Max, this.dim2.Max, false, true))
            {
                regions.Add(region);
            }


            regions.TrimExcess();
            return regions;
        }
        #endregion

        #region Private Members

        private bool TryCreateRegion(
            out SpacialRegion<T1, T2> region,
            T1 dim1Min, T1 dim1Max, bool dim1MinIncluded, bool dim1MaxIncluded,
            T2 dim2Min, T2 dim2Max, bool dim2MinIncluded, bool dim2MaxIncluded)
        {
            //Validate dimension 1
            if (spacialDefinition.Dimension1.Compare(dim1Min, dim1Max) > 0)
            {
                region = default(SpacialRegion<T1, T2>);
                return false;
            }

            //Validate dimension 2
            if (spacialDefinition.Dimension2.Compare(dim2Min, dim2Max) > 0)
            {
                region = default(SpacialRegion<T1, T2>);
                return false;
            }


            region = new SpacialRegion<T1, T2>(
                this.spacialDefinition,
                new Range<T1>(dim1Min, dim1Max, dim1MinIncluded, dim1MaxIncluded),
                new Range<T2>(dim2Min, dim2Max, dim2MinIncluded, dim2MaxIncluded));

            return true;
        }
        #endregion
    }

    public struct SpacialRegion<T1, T2, T3>
    {
        #region Fields

        private readonly SpacialDefinition<T1, T2, T3> spacialDefinition;

        private readonly Range<T1> dim1;
        private readonly Range<T2> dim2;
        private readonly Range<T3> dim3;
        #endregion

        #region Constructors

        public SpacialRegion(
            SpacialDefinition<T1, T2, T3> spacialDefinition,
            Range<T1> dim1,
            Range<T2> dim2,
            Range<T3> dim3)
        {
            //Validation
            if (spacialDefinition == null)
                throw new ArgumentNullException("spacialDefinition");


            this.spacialDefinition = spacialDefinition;

            this.dim1 = dim1;
            this.dim2 = dim2;
            this.dim3 = dim3;
        }
        #endregion

        #region Public Methods

        [Pure]
        public bool DoesRegionOverlap(SpacialRegion<T1, T2, T3> other)
        {
            return
                this.spacialDefinition.Dimension1.DoDimensionsOverlap(this.dim1, other.dim1) &&
                this.spacialDefinition.Dimension2.DoDimensionsOverlap(this.dim2, other.dim2) &&
                this.spacialDefinition.Dimension3.DoDimensionsOverlap(this.dim3, other.dim3);
        }

        [Pure]
        public bool IsPointInRegion(Tuple<T1, T2, T3> point)
        {
            return
                spacialDefinition.Dimension1.IsPointInDimension(point.Item1, this.dim1) &&
                spacialDefinition.Dimension2.IsPointInDimension(point.Item2, this.dim2) &&
                spacialDefinition.Dimension3.IsPointInDimension(point.Item3, this.dim3);
        }

        [Pure]
        public IEnumerable<SpacialRegion<T1, T2, T3>> Subtract(SpacialRegion<T1, T2, T3> other)
        {
            //Ensure the SpacialRegions pertain to the same key space.
            if (this.spacialDefinition == other.spacialDefinition)
                throw new ArgumentException( /* TODO: error message */ );


            SpacialRegion<T1, T2, T3> region;
            List<SpacialRegion<T1, T2, T3>> regions = new List<SpacialRegion<T1, T2, T3>>(2 * 4);

            // D1
            if (TryCreateRegion(
                out region,
                this.dim1.Min, other.dim1.Min, true, false,
                this.dim2.Min, this.dim2.Max, true, true,
                this.dim3.Min, this.dim3.Max, true, true))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Max, this.dim1.Max, false, true,
                this.dim2.Min, this.dim2.Max, true, true,
                this.dim3.Min, this.dim3.Max, true, true))
            {
                regions.Add(region);
            }

            // D2
            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                this.dim2.Min, other.dim2.Min, true, false,
                this.dim3.Min, this.dim3.Max, true, true))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Max, this.dim2.Max, false, true,
                this.dim3.Min, this.dim3.Max, true, true))
            {
                regions.Add(region);
            }

            // D3
            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Min, other.dim2.Max, true, true,
                this.dim3.Min, other.dim3.Min, true, false))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Min, other.dim2.Max, true, true,
                other.dim3.Max, this.dim3.Max, false, true))
            {
                regions.Add(region);
            }

            regions.TrimExcess();
            return regions;
        }
        #endregion

        #region Private Members

        private bool TryCreateRegion(
            out SpacialRegion<T1, T2, T3> region,
            T1 dim1Min, T1 dim1Max, bool dim1MinIncluded, bool dim1MaxIncluded,
            T2 dim2Min, T2 dim2Max, bool dim2MinIncluded, bool dim2MaxIncluded,
            T3 dim3Min, T3 dim3Max, bool dim3MinIncluded, bool dim3MaxIncluded)
        {
            //Validate dimension 1
            if (spacialDefinition.Dimension1.Compare(dim1Min, dim1Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3>);
                return false;
            }

            //Validate dimension 2
            if (spacialDefinition.Dimension2.Compare(dim2Min, dim2Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3>);
                return false;
            }

            //Validate dimension 3
            if (spacialDefinition.Dimension3.Compare(dim3Min, dim3Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3>);
                return false;
            }


            region = new SpacialRegion<T1, T2, T3>(
                this.spacialDefinition,
                new Range<T1>(dim1Min, dim1Max, dim1MinIncluded, dim1MaxIncluded),
                new Range<T2>(dim2Min, dim2Max, dim2MinIncluded, dim2MaxIncluded),
                new Range<T3>(dim3Min, dim3Max, dim3MinIncluded, dim3MaxIncluded));

            return true;
        }
        #endregion
    }

    public struct SpacialRegion<T1, T2, T3, T4>
    {
        #region Fields

        private readonly SpacialDefinition<T1, T2, T3, T4> spacialDefinition;

        private readonly Range<T1> dim1;
        private readonly Range<T2> dim2;
        private readonly Range<T3> dim3;
        private readonly Range<T4> dim4;
        #endregion

        #region Constructors

        public SpacialRegion(
            SpacialDefinition<T1, T2, T3, T4> spacialDefinition,
            Range<T1> dim1,
            Range<T2> dim2,
            Range<T3> dim3,
            Range<T4> dim4)
        {
            //Validation
            if (spacialDefinition == null)
                throw new ArgumentNullException("spacialDefinition");


            this.spacialDefinition = spacialDefinition;

            this.dim1 = dim1;
            this.dim2 = dim2;
            this.dim3 = dim3;
            this.dim4 = dim4;
        }
        #endregion

        #region Public Methods

        [Pure]
        public bool DoesRegionOverlap(SpacialRegion<T1, T2, T3, T4> other)
        {
            return
                this.spacialDefinition.Dimension1.DoDimensionsOverlap(this.dim1, other.dim1) &&
                this.spacialDefinition.Dimension2.DoDimensionsOverlap(this.dim2, other.dim2) &&
                this.spacialDefinition.Dimension3.DoDimensionsOverlap(this.dim3, other.dim3) &&
                this.spacialDefinition.Dimension4.DoDimensionsOverlap(this.dim4, other.dim4);
        }

        [Pure]
        public bool IsPointInRegion(Tuple<T1, T2, T3, T4> point)
        {
            return
                spacialDefinition.Dimension1.IsPointInDimension(point.Item1, this.dim1) &&
                spacialDefinition.Dimension2.IsPointInDimension(point.Item2, this.dim2) &&
                spacialDefinition.Dimension3.IsPointInDimension(point.Item3, this.dim3) &&
                spacialDefinition.Dimension4.IsPointInDimension(point.Item4, this.dim4);
        }

        [Pure]
        public IEnumerable<SpacialRegion<T1, T2, T3, T4>> Subtract(SpacialRegion<T1, T2, T3, T4> other)
        {
            //Ensure the SpacialRegions pertain to the same key space.
            if (this.spacialDefinition == other.spacialDefinition)
                throw new ArgumentException( /* TODO: error message */ );


            SpacialRegion<T1, T2, T3, T4> region;
            List<SpacialRegion<T1, T2, T3, T4>> regions = new List<SpacialRegion<T1, T2, T3, T4>>(2 * 4);

            // D1
            if (TryCreateRegion(
                out region,
                this.dim1.Min, other.dim1.Min, true, false,
                this.dim2.Min, this.dim2.Max, true, true,
                this.dim3.Min, this.dim3.Max, true, true,
                this.dim4.Min, this.dim4.Max, true, true))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Max, this.dim1.Max, false, true,
                this.dim2.Min, this.dim2.Max, true, true,
                this.dim3.Min, this.dim3.Max, true, true,
                this.dim4.Min, this.dim4.Max, true, true))
            {
                regions.Add(region);
            }

            // D2
            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                this.dim2.Min, other.dim2.Min, true, false,
                this.dim3.Min, this.dim3.Max, true, true,
                this.dim4.Min, this.dim4.Max, true, true))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Max, this.dim2.Max, false, true,
                this.dim3.Min, this.dim3.Max, true, true,
                this.dim4.Min, this.dim4.Max, true, true))
            {
                regions.Add(region);
            }

            // D3
            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Min, other.dim2.Max, true, true,
                this.dim3.Min, other.dim3.Min, true, false,
                this.dim4.Min, this.dim4.Min, true, true))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Min, other.dim2.Max, true, true,
                other.dim3.Max, this.dim3.Max, false, true,
                this.dim4.Min, this.dim4.Max, true, true))
            {
                regions.Add(region);
            }

            // D4
            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Min, other.dim2.Max, true, true,
                other.dim3.Min, other.dim3.Max, true, true,
                this.dim4.Min, other.dim4.Min, true, false))
            {
                regions.Add(region);
            }

            if (TryCreateRegion(
                out region,
                other.dim1.Min, other.dim1.Max, true, true,
                other.dim2.Min, other.dim2.Max, true, true,
                other.dim3.Min, other.dim3.Max, true, true,
                other.dim4.Max, this.dim4.Max, false, true))
            {
                regions.Add(region);
            }

            regions.TrimExcess();
            return regions;
        }
        #endregion

        #region Private Members

        private bool TryCreateRegion(
            out SpacialRegion<T1, T2, T3, T4> region,
            T1 dim1Min, T1 dim1Max, bool dim1MinIncluded, bool dim1MaxIncluded,
            T2 dim2Min, T2 dim2Max, bool dim2MinIncluded, bool dim2MaxIncluded,
            T3 dim3Min, T3 dim3Max, bool dim3MinIncluded, bool dim3MaxIncluded,
            T4 dim4Min, T4 dim4Max, bool dim4MinIncluded, bool dim4MaxIncluded)
        {
            //Validate dimension 1
            if (spacialDefinition.Dimension1.Compare(dim1Min, dim1Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3, T4>);
                return false;
            }

            //Validate dimension 2
            if (spacialDefinition.Dimension2.Compare(dim2Min, dim2Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3, T4>);
                return false;
            }

            //Validate dimension 3
            if (spacialDefinition.Dimension3.Compare(dim3Min, dim3Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3, T4>);
                return false;
            }

            //Validate dimension 4
            if (spacialDefinition.Dimension4.Compare(dim4Min, dim4Max) > 0)
            {
                region = default(SpacialRegion<T1, T2, T3, T4>);
                return false;
            }


            region = new SpacialRegion<T1, T2, T3, T4>(
                this.spacialDefinition,
                new Range<T1>(dim1Min, dim1Max, dim1MinIncluded, dim1MaxIncluded),
                new Range<T2>(dim2Min, dim2Max, dim2MinIncluded, dim2MaxIncluded),
                new Range<T3>(dim3Min, dim3Max, dim3MinIncluded, dim3MaxIncluded),
                new Range<T4>(dim4Min, dim4Max, dim4MinIncluded, dim4MaxIncluded));

            return true;
        }
        #endregion
    }
}
