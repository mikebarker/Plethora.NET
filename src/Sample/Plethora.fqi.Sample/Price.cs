using System;

namespace Plethora.fqi.Sample
{
    class Price
    {
        private static readonly Random rnd = new Random(173);

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
}
