using System;
using System.Collections.Generic;

using Plethora.Cache.Spacial;
using Plethora.Collections.Sets;
using Plethora.Spacial;

namespace Plethora.Cache.Sample.ComplexExample
{
    public class PriceArg : SpacialArgumentBase<Price, PriceArg, string, DateTime>
    {
        public PriceArg(string tickerSymbol, DateTime minDate, DateTime maxDate)
            : this(new SpaceRegion<string, DateTime>(
                new InclusiveSet<string>(tickerSymbol),
                new RangeInclusiveSet<DateTime>(minDate, maxDate)))
        {
        }

        private PriceArg(SpaceRegion<string, DateTime> region)
            : base(GetKeyPointFromData, region)
        {
        }

        protected override PriceArg CreateArgWithRegion(SpaceRegion<string, DateTime> newRegion)
        {
            return new PriceArg(newRegion);
        }

        private static Tuple<string, DateTime> GetKeyPointFromData(Price price)
        {
            return new Tuple<string, DateTime>(price.TickerSymbol, price.Date);
        }


        public IEnumerable<string> TickerSymbols
        {
            get { return ((InclusiveSet<string>)base.Region.Dimension1).IncludedElements; }
        }

        public DateTime MinDate
        {
            get { return ((RangeInclusiveSet<DateTime>)base.Region.Dimension2).Range.Min; }
        }

        public DateTime MaxDate
        {
            get { return ((RangeInclusiveSet<DateTime>)base.Region.Dimension2).Range.Max; }
        }
    }
}
