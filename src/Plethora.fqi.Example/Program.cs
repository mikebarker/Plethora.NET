using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Plethora.fqi.Example
{
    static class Program
    {
        const int ELEMENT_COUNT = 1000000;
        const int LOOKUPS = 10000;

        static readonly Random rnd = new Random(173);
        static List<Price> collection;
        static readonly PriceArg filterArgument = new PriceArg();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Setup();

            ProfileList();
            ProfileMultiIndexCollection();
        }

        static void Setup()
        {
            Console.WriteLine("Generating random Price elements...");

            //Keep generation of price objects outside the population loop so that
            // population times are not skewed by creation time
            collection = new List<Price>(ELEMENT_COUNT);
            for (int i = 0; i < ELEMENT_COUNT; i++)
            {
                var price = Price.GenerateRandom();

                collection.Add(price);
            }

            Console.WriteLine("Done.");
            Console.WriteLine();
        }

        static void TestPopulation(ICollection<Price> prices)
        {
            Console.WriteLine(" Adding {0} objects to collection.", ELEMENT_COUNT);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            foreach (var price in collection)
            {
                prices.Add(price);
            }
            watch.Stop();

            Console.WriteLine(" Done.");
            Console.WriteLine(" Add duration: {0:N3} seconds", (watch.ElapsedMilliseconds/1000.0));
        }

        static void TestRandomLookup(IEnumerable<Price> filteredPrices)
        {
            Console.WriteLine(" Executing {0} random lookups over {1} objects...", LOOKUPS, ELEMENT_COUNT);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < LOOKUPS; i++)
            {
                int c = rnd.Next(0, ELEMENT_COUNT);
                filterArgument.Currency = collection[c].Currency;
                filterArgument.PriceDate = collection[c].PriceDate;

                var item = filteredPrices.First();
            }
            watch.Stop();

            Console.WriteLine(" Done.");
            Console.WriteLine(" Random lookup duration: {0:N3} seconds", (watch.ElapsedMilliseconds / 1000.0));
        }

        static void ProfileList()
        {
            var prices = new List<Price>();

            var filteredPrices = prices
                .Where(r => (r.PriceDate == filterArgument.PriceDate))
                .Where(r => (r.Currency == filterArgument.Currency));


            Console.WriteLine("Testing collection type {0}.", prices.GetType().Name);

            Console.WriteLine("Population test");
            TestPopulation(prices);

            Console.WriteLine();
            Console.WriteLine("Lookup test");

            TestRandomLookup(filteredPrices);

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        static void ProfileMultiIndexCollection()
        {
            var spec = new MultiIndexSpecification<Price>();
            spec
                .AddIndex(false, p => p.PriceDate).Then(p => p.Currency);

            var prices = new MultiIndexedCollection<Price>(spec);


            var filteredPrices = prices
                .Where(r => (r.PriceDate == filterArgument.PriceDate))
                .Where(r => (r.Currency == filterArgument.Currency));


            Console.WriteLine("Testing collection type {0}.", prices.GetType().Name);

            Console.WriteLine("Population test");
            TestPopulation(prices);

            Console.WriteLine();
            Console.WriteLine("Lookup test");

            TestRandomLookup(filteredPrices);

            Console.WriteLine();
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

            private static DateTime GetRandomDate()
            {
                return new DateTime(rnd.Next(2005, 2010), rnd.Next(1, 13), rnd.Next(1, 28));
            }

            private static string GetRandomCcy()
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
