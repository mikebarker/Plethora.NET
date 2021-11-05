using Plethora.Cache.Sample.ComplexExample;
using Plethora.Cache.Spacial;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Cache.Sample.BuilderExample
{
    static class BuilderProgram
    {
        public static async Task RunAsync()
        {
            Console.Write("Cache hits (when the data requested is in the cache) return ");
            Console.Write("almost instantaneously; whilst cache misses take time for the ");
            Console.Write("data source to return the data requested.");
            Console.WriteLine();
            Console.WriteLine();

            // Define the cache
            var builder = CacheBuilder<Price>
                .AddDiscreteDimension(price => price.TickerSymbol)
                .AddRangeDimension(price => price.Date);

            PriceSource source = new PriceSource();

            // Build the cache to utilise the PriceSource
            Task<IEnumerable<Price>> getPricesCallback(IEnumerable<Tuple<string, Range<DateTime>>> args, CancellationToken cancellationToken)
            {
                var task = Task.Run(() =>
                {
                    List<Price> list = new List<Price>();
                    foreach (var arg in args)
                    {
                        var tickerSymbols = arg.Item1;
                        var dateRange = arg.Item2;

                        var prices = source.GetPrices(tickerSymbols, dateRange.Min, dateRange.Max);
                        list.AddRange(prices);
                    }

                    return (IEnumerable<Price>)list;
                });

                return task;
            }


            var cache = builder.CreateCache(getPricesCallback);

            // Example of calling the cache
            IEnumerable<Price> prices;

            Console.Write("Getting prices for stock MSFT, from 2004-03-16 - 2005-09-10... ");
            var arg = builder.CreateArg(
                "MSFT",
                new Range<DateTime>(new DateTime(2004, 03, 16), new DateTime(2005, 09, 10)));

            prices = await cache.GetDataAsync(arg).ConfigureAwait(false);
            Console.WriteLine("done.");

            Console.Write("Getting prices for stock MSFT, from 2004-05-01 - 2004-05-20... ");
            arg = builder.CreateArg(
                "MSFT",
                new Range<DateTime>(new DateTime(2004, 05, 01), new DateTime(2004, 05, 20)));

            prices = await cache.GetDataAsync(arg).ConfigureAwait(false);
            Console.WriteLine("done.");

            foreach (Price price in prices)
            {
                Console.WriteLine($"{price.TickerSymbol} [{price.Date.ToString("yyyy-MM-dd")}] = {price.Value}");
            }
        }
    }
}
