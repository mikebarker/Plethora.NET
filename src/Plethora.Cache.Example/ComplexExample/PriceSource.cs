using System;
using System.Collections.Generic;
using System.Threading;

namespace Plethora.Cache.Example.ComplexExample
{
    public class PriceSource
    {
        private static readonly Random rnd = new Random();

        private readonly Dictionary<long, Dictionary<DateTime, Price>> prices =
            new Dictionary<long, Dictionary<DateTime, Price>>();

        
        public PriceSource()
        {
            //Generate random prices for stock IDs 0 - 100, for dates 2000-01-01 - 2010-12-31
            for (long i = 0; i <= 100; i++)
            {
                var stockPrices = new Dictionary<DateTime, Price>();
                this.prices.Add(i, stockPrices);

                for (DateTime date = new DateTime(2000, 01, 01); date < new DateTime(2010, 12, 31); date=date.AddDays(1))
                {
                    Price price = new Price(i, date, (decimal)rnd.NextDouble());
                    stockPrices.Add(date, price);
                }
            }
        }

        public List<Price> GetPrices(long stockId, DateTime minDate, DateTime maxDate)
        {
            Thread.Sleep(3000);

            List<Price> rtn = new List<Price>();
            for (DateTime date = minDate; date <= maxDate; date = date.AddDays(1))
            {
                rtn.Add(this.prices[stockId][date]);
            }
            return rtn;
        }
    }
}
