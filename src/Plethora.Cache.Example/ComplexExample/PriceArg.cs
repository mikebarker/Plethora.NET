using System;
using System.Collections.Generic;
using Plethora.Cache.Spacial;
using Plethora.Collections.Sets;

namespace Plethora.Cache.Example.ComplexExample
{
    public class PriceArg : SpacialArgument<Price, PriceArg, long, DateTime>
    {
        public PriceArg(long stockId, DateTime minDate, DateTime maxDate)
            : this(new SpaceRegion<long, DateTime>(
                new InclusiveSet<long>(stockId),
                new RangeSet<DateTime>(minDate, maxDate)))
        {
        }

        private PriceArg(SpaceRegion<long, DateTime> region)
            : base(GetKeyPointFromData, region)
        {
        }

        public PriceArg()
            : base(GetKeyPointFromData)
        {
        }


        private static Tuple<long, DateTime> GetKeyPointFromData(Price price)
        {
            return new Tuple<long, DateTime>(price.StockId, price.Date);
        }


        public IEnumerable<long> StockIds
        {
            get { return ((InclusiveSet<long>)base.Region.Dimension1).IncludedElements; }
        }

        public DateTime MinDate
        {
            get { return ((RangeSet<DateTime>)base.Region.Dimension2).Min; }
        }

        public DateTime MaxDate
        {
            get { return ((RangeSet<DateTime>)base.Region.Dimension2).Max; }
        }
    }
}
