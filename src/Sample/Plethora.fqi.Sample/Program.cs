using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Plethora.fqi.Sample
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
    }
}
