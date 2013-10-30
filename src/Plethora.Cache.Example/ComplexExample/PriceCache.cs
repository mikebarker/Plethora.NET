using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Cache.Example.ComplexExample
{
    public class PriceCache : CacheBase<Price, PriceArg>
    {
        private readonly PriceSource source = new PriceSource();

        public IEnumerable<Price> GetStockPrices(int stockId, DateTime from, DateTime to)
        {
            PriceArg arg = new PriceArg(stockId, from, to);

            List<PriceArg> arguments = new List<PriceArg> { arg };
            IEnumerable<Price> prices = GetData(arguments, 5000);
            return prices;
        }

        #region Overrides of CacheBase<Price,PriceArg>

        /// <summary>
        /// Fetches the required data from the data source.
        /// </summary>
        protected override IEnumerable<Price> GetDataFromSource(IEnumerable<PriceArg> arguments, int millisecondsTimeout)
        {
            IEnumerable<Price> prices = Enumerable.Empty<Price>();
            foreach (PriceArg argument in arguments)
            {
                foreach (long stockId in argument.StockIds)
                {
                    prices = prices.Concat(this.source.GetPrices(stockId, argument.MinDate, argument.MaxDate));
                }
            }
            return prices;
        }

        #endregion
    }
}
