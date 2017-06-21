using System;

namespace Plethora.Cache.Sample.ComplexExample
{
    /// <summary>
    /// A price represents a stock price, which is uniquely keyed on the stock's ID and the price date
    /// </summary>
    public class Price
    {
        private readonly long stockId;
        private readonly DateTime date;
        private readonly decimal value;

        public Price(long stockId, DateTime date, decimal value)
        {
            this.stockId = stockId;
            this.date = date;
            this.value = value;
        }

        public long StockId
        {
            get { return this.stockId; }
        }

        public DateTime Date
        {
            get { return this.date; }
        }

        public decimal Value
        {
            get { return this.value; }
        }
    }
}
