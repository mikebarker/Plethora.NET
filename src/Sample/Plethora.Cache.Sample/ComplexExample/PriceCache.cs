using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Cache.Sample.ComplexExample
{
    public class PriceCache : CacheBase<Price, PriceArg>
    {
        private readonly PriceSource source = new PriceSource();

        public async Task<IEnumerable<Price>> GetStockPricesAsync(string tickerSymbol, DateTime from, DateTime to)
        {
            PriceArg arg = new PriceArg(tickerSymbol, from, to);

            List<PriceArg> arguments = new List<PriceArg> { arg };
            IEnumerable<Price> prices = await this.GetDataAsync(arguments).ConfigureAwait(false);
            return prices;
        }

        #region Overrides of CacheBase<Price,PriceArg>

        /// <summary>
        /// Fetches the required data from the data source.
        /// </summary>
        protected override async Task<IEnumerable<Price>> GetDataFromSourceAsync(IEnumerable<PriceArg> arguments, CancellationToken cancellationToken = default)
        {
            IEnumerable<Price> prices = Enumerable.Empty<Price>();
            foreach (PriceArg argument in arguments)
            {
                foreach (string tickerSymbol in argument.TickerSymbols)
                {
                    prices = prices.Concat(this.source.GetPrices(tickerSymbol, argument.MinDate, argument.MaxDate));
                }
            }
            return prices;
        }

        #endregion
    }
}
