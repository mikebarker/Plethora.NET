using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plethora.Cache.Sample.ComplexExample
{
    static class ComplexProgram
    {
        public static async Task RunAsync()
        {
            Console.Write("Cache hits (when the data requested is in the cache) return ");
            Console.Write("almost instantaneously; whilst cache misses take time for the ");
            Console.Write("data source to return the data requested.");
            Console.WriteLine();
            Console.WriteLine();

            PriceCache cache = new PriceCache();

            IEnumerable<Price> prices;

            Console.Write("Getting prices for stock MSFT, from 2004-03-16 - 2005-09-10... ");
            prices = await cache.GetStockPricesAsync("MSFT", new DateTime(2004, 03, 16), new DateTime(2005, 09, 10)).ConfigureAwait(false);
            Console.WriteLine("done.");

            Console.Write("Getting prices for stock MSFT, from 2004-05-01 - 2004-05-20... ");
            prices = await cache.GetStockPricesAsync("MSFT", new DateTime(2004, 05, 01), new DateTime(2004, 05, 20)).ConfigureAwait(false);
            Console.WriteLine("done.");

            foreach (Price price in prices)
            {
                Console.WriteLine($"{price.TickerSymbol} [{price.Date.ToString("yyyy-MM-dd")}] = {price.Value}");
            }
        }
    }
}
