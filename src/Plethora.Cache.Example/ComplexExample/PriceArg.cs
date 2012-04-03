using System;
using System.Collections.Generic;

namespace Plethora.Cache.Example.ComplexExample
{
    public class PriceArg : IArgument<Price, PriceArg>
    {
        private readonly long minStockId;
        private readonly long maxStockId;
        private readonly DateTime minDate;
        private readonly DateTime maxDate;

        public PriceArg(long minStockId, long maxStockId, DateTime minDate, DateTime maxDate)
        {
            this.minStockId = minStockId;
            this.maxStockId = maxStockId;
            this.minDate = minDate;
            this.maxDate = maxDate;
        }

        #region Properties

        public long MinStockId
        {
            get { return minStockId; }
        }

        public long MaxStockId
        {
            get { return maxStockId; }
        }

        public DateTime MinDate
        {
            get { return minDate; }
        }

        public DateTime MaxDate
        {
            get { return maxDate; }
        }
        #endregion

        #region Implementation of IArgument<Price, PriceArg>

        /// <summary>
        /// Gets a value indicating whether two arguments overlap in the key-space which they represent.
        /// </summary>
        public bool IsOverlapped(PriceArg B, out IEnumerable<PriceArg> notInB)
        {
            // (For purposes of this explanation the region covered by 'this' will be refered to as A.)
            //
            // One may consider 'A'-'B' to be four possible regions. Any, all, or none of which may not
            // be valid. We'll name these regions 'a', 'b', 'c', and 'd'. These regions are defined as*:
            //  'a' is the space bound by A.minStockId to B.minStockId-1, and A.minDate to A.maxDate
            //  'b' is the space bound by B.minStockId to B.maxStockId, and B.maxDate+1 to A.maxDate
            //  'c' is the space bound by B.minStockId to B.maxStockId, and A.minDate to B.minDate-1
            //  'd' is the space bound by B.maxStockId+1 to A.maxStockId, and A.minDate to A.maxDate
            //
            // * where "1" in the above definitions is a single "unit" of dimension (i.e. one day in the case
            //   of Date, and 1 in the case of IDs)
            // The regions may be visualised as (where A is the larger bordered region, and B is the smaller):
            //
            //     +---------------------+
            //  S  |??????@@@@@@@@@@@\\\\|
            //  t  |??????@@@@ b @@@@\\\\|
            //  o  |??????@@@@@@@@@@@\\\\|
            //  c  |??????+---------+\\\\|
            //  k  |? a ??|         |\ d |
            //     |??????|         |\\\\|
            //  I  |??????+---------+\\\\|
            //  d  |??????###########\\\\|
            //     |??????#### c ####\\\\|
            //     +---------------------+
            //               Date
            // 
            // For each of these defined regions ('a'-'d'), the measures derived from 'B' must be limited
            // by the bounds of 'A'. That is, if A.maxDate < B.minDate-1 then the region 'c' must be limited
            // from A.minDate to A.maxDate (and not to B.minDate-1)
            //
            // Where a region's minimum (Date or StockId) exceeds it's maximum, the region is not valid
            // and must not be returned. e.g. Consider the case:
            //          A
            //     +----------+
            //     |??????@@b@|    B
            //     |??????+---|-------+
            //     |? a ??|   |       |
            //     |??????+---|-------+
            //     |??????##c#|
            //     +----------+
            // In this case regions 'b' and 'c' are bound by the region 'A'. Note also that 'd' is not a valid
            // region in this case, as 'A' has no content in that area of the key-space.

            //Region 'a'
            long region_a_minStockId = this.minStockId;
            long region_a_maxStockId = Min(B.minStockId-1, this.maxStockId);
            DateTime region_a_minDate = this.minDate;
            DateTime region_a_maxDate = this.maxDate;

            //Region 'b'
            long region_b_minStockId = Max(B.minStockId, this.minStockId);
            long region_b_maxStockId = Min(B.maxStockId, this.maxStockId);
            DateTime region_b_minDate = Max(B.maxDate.AddDays(1), this.minDate);
            DateTime region_b_maxDate = this.maxDate;

            //Region 'c'
            long region_c_minStockId = Max(B.minStockId, this.minStockId);
            long region_c_maxStockId = Min(B.maxStockId, this.maxStockId);
            DateTime region_c_minDate = this.minDate;
            DateTime region_c_maxDate = Min(B.minDate.AddDays(-1), this.maxDate);

            //Region 'd'
            long region_d_minStockId = Max(B.maxStockId+1, this.minStockId);
            long region_d_maxStockId = this.maxStockId;
            DateTime region_d_minDate = this.minDate;
            DateTime region_d_maxDate = this.maxDate;

            List<PriceArg> regions = new List<PriceArg>(4);

            if ((region_a_minStockId <= region_a_maxStockId) && (region_a_minDate <= region_a_maxDate))
                regions.Add(new PriceArg(region_a_minStockId, region_a_maxStockId, region_a_minDate, region_a_maxDate));

            if ((region_b_minStockId <= region_b_maxStockId) && (region_b_minDate <= region_b_maxDate))
                regions.Add(new PriceArg(region_b_minStockId, region_b_maxStockId, region_b_minDate, region_b_maxDate));

            if ((region_c_minStockId <= region_c_maxStockId) && (region_c_minDate <= region_c_maxDate))
                regions.Add(new PriceArg(region_c_minStockId, region_c_maxStockId, region_c_minDate, region_c_maxDate));

            if ((region_d_minStockId <= region_d_maxStockId) && (region_d_minDate <= region_d_maxDate))
                regions.Add(new PriceArg(region_d_minStockId, region_d_maxStockId, region_d_minDate, region_d_maxDate));

            //If there is only one of the 'a', 'b', 'c' or 'd' regions which is valid, then that region
            // must have the same bounds as 'A'... meaning that 'A' and 'B' do not overlap.
            if (regions.Count == 1)
            {
                notInB = null;
                return false;
            }

            //Shrink regions
            regions.Capacity = regions.Count;

            notInB = regions;
            return true;
        }

        /// <summary>
        /// A filtering function which returns a flag indicating whether data is represented by this
        /// argument instance.
        /// </summary>
        public bool IsDataIncluded(Price data)
        {
            if ((this.minStockId <= data.StockId) && (data.StockId <= this.maxStockId) &&
                (this.minDate <= data.Date) && (data.Date <= this.maxDate))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Boundary Methods

        private static long Min(long a, long b)
        {
            return (b < a) ? b : a;
        }

        private static long Max(long a, long b)
        {
            return (b > a) ? b : a;
        }

        private static DateTime Min(DateTime a, DateTime b)
        {
            return (b < a) ? b : a;
        }

        private static DateTime Max(DateTime a, DateTime b)
        {
            return (b > a) ? b : a;
        }
        #endregion
    }
}
