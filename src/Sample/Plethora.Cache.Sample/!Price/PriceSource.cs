using System;
using System.Collections.Generic;
using System.Threading;

namespace Plethora.Cache.Sample
{
    public class PriceSource
    {
        private static readonly Random rnd = new Random();

        private readonly Dictionary<string, Dictionary<DateTime, Price>> prices =
            new Dictionary<string, Dictionary<DateTime, Price>>();

        string[] tickerSymbols = new[]
        {
            "AAPL",
            "MSFT",
            "VOD",
            "RDSA",
            "RDSB",
            "GOLD",
            "CAT",
            "RED.L",
        };


        public PriceSource()
        {
            //Generate random prices for stock IDs 0 - 100, for dates 2000-01-01 - 2010-12-31
            foreach(string tickerSymbol in tickerSymbols)
            {
                var stockPrices = new Dictionary<DateTime, Price>();
                this.prices.Add(tickerSymbol, stockPrices);

                for (DateTime date = new DateTime(2000, 01, 01); date < new DateTime(2010, 12, 31); date=date.AddDays(1))
                {
                    Price price = new Price(tickerSymbol, date, (decimal)rnd.NextDouble());
                    stockPrices.Add(date, price);
                }
            }
        }

        public List<Price> GetPrices(string tickerSymbol, DateTime minDate, DateTime maxDate)
        {
            // Simulate the source taking some time return a result
            Thread.Sleep(3000);

            List<Price> rtn = new List<Price>();
            for (DateTime date = minDate; date <= maxDate; date = date.AddDays(1))
            {
                rtn.Add(this.prices[tickerSymbol][date]);
            }
            return rtn;
        }
    }
}
