//Comment out or delete to execute List<T>, include to execute with MultiIndexCollection<T>
#define USE_INDEX

using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.fqi.Example
{
    static class Program
    {
        static readonly Random rnd = new Random(173);

        static void Profile()
        {
            const int ELEMENT_COUNT = 1000000;
            const int LOOKUPS = 10000;

            Console.WriteLine("Generating random Price elements.");

            //Keep generation of price objects outside the population loop so that
            // population times are not skewed by creation time
            List<Price> collection = new List<Price>(ELEMENT_COUNT);
            for (int i = 0; i < ELEMENT_COUNT; i++)
            {
                var price = Price.GenerateRandom();

                collection.Add(price);
            }

#if USE_INDEX
            var spec = new MultiIndexSpecification<Price>();
            spec
                .AddIndex(false, p => p.PriceDate).Then(p => p.Currency);

            var prices = new MultiIndexedCollection<Price>(spec);
#else
            var prices = new List<Price>();
#endif
            Console.WriteLine("Utilising collection type {0}.", prices.GetType().Name);
            Console.WriteLine();
            Console.WriteLine("Adding {0} objects to collection.", ELEMENT_COUNT);

            DateTime start0 = DateTime.Now;
            foreach (var price in collection)
            {
                prices.Add(price);
            }
            DateTime stop0 = DateTime.Now;
            TimeSpan duration0 = stop0 - start0;


            Console.WriteLine("Add Duration: {0}", duration0.ToString());
            Console.WriteLine();

            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            Console.WriteLine("Go...");

            Console.WriteLine();
            Console.WriteLine("Executing {0} random lookups over {1} objects...", LOOKUPS, ELEMENT_COUNT);

            //Again, keep constructions outside from the loop to prevent
            // skewing of results
            PriceArg arg = new PriceArg(); //Pick a price at random
            var f = prices.Where(r => (r.PriceDate == arg.PriceDate)).Where(r => (r.Currency == arg.Currency));

            DateTime start2 = DateTime.Now;
            for (int i = 0; i < LOOKUPS; i++)
            {
                int c = rnd.Next(0, ELEMENT_COUNT);
                arg.Currency = collection[c].Currency;
                arg.PriceDate = collection[c].PriceDate;

                var g = f.First();
            }
            DateTime stop2 = DateTime.Now;
            TimeSpan duration2 = stop2 - start2;

            Console.WriteLine("Random lookup duration: {0}", duration2.ToString());
            Console.WriteLine();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Profile();

            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        class Price
        {
            public long InstrumentId { get; private set; }
            public decimal PriceValue { get; private set; }
            public DateTime PriceDate { get; private set; }
            public string Currency { get; private set; }

            public Price(long instrumentId, decimal priceValue, DateTime priceDate, string currency)
            {
                this.InstrumentId = instrumentId;
                this.PriceValue = priceValue;
                this.Currency = currency;
                this.PriceDate = priceDate;
            }

            public static Price GenerateRandom()
            {
                long instrumentId;
                decimal priceValue;
                DateTime priceDate;
                string currency;

                //Offset the weight of instrument to 0 (ie. non-std distribution)
                int value = rnd.Next(0, 100);
                if (value > 98)
                    instrumentId = rnd.Next(100, 10000);
                else
                    instrumentId = 0;

                priceValue = (decimal)(rnd.NextDouble() * 1000000.0f);
                priceDate = GetRandomDate();
                currency = GetRandomCcy();

                return new Price(instrumentId, priceValue, priceDate, currency);
            }

            public static DateTime GetRandomDate()
            {
                return new DateTime(rnd.Next(2005, 2010), rnd.Next(1, 13), rnd.Next(1, 28));
            }

            public static string GetRandomCcy()
            {
                var value = rnd.Next(0, 4);
                if (value == 0)
                    return "GBP";
                else if (value == 1)
                    return "HKD";
                else
                    return "USD";
            }
        }

        class PriceArg
        {
            public DateTime PriceDate { get; set; }
            public string Currency { get; set; }
        }

    }
}
