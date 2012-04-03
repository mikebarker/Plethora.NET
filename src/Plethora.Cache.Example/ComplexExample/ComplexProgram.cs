using System;
using System.Collections.Generic;

namespace Plethora.Cache.Example.ComplexExample
{
    static class ComplexProgram
    {
        public static void Run()
        {
            Console.Write("Cache hits (when the data requested is in the cache) return ");
            Console.Write("almost instantaneously; whilst cache misses take time for the ");
            Console.Write("data source to return the data requested.");
            Console.WriteLine();
            Console.WriteLine();

            PriceCache cache = new PriceCache();

            IEnumerable<Price> prices;

            Console.Write("Getting prices for stock 54, from 2004-03-16 - 2005-09-10... ");
            prices = cache.GetStockPrices(54, new DateTime(2004, 03, 16), new DateTime(2005, 09, 10));
            Console.WriteLine("done.");

            Console.Write("Getting prices for stock 54, from 2004-05-01 - 2004-05-20... ");
            prices = cache.GetStockPrices(54, new DateTime(2004, 05, 01), new DateTime(2004, 05, 20));
            Console.WriteLine("done.");

            foreach (Price price in prices)
            {
                Console.WriteLine("{0} [{1}] = {2}", price.StockId, price.Date.ToString("yyyy-MM-dd"), price.Value);
            }
        }
    }
}
