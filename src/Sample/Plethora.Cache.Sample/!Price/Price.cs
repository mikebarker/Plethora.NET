using System;

namespace Plethora.Cache.Sample
{
    /// <summary>
    /// A price represents a stock price, which is uniquely keyed on the stock's ticker-symbol and the price date (i.e. 2 key dimensions).
    /// </summary>
    public class Price
    {
        public Price(string tickerSymbol, DateTime date, decimal value)
        {
            this.TickerSymbol = tickerSymbol;
            this.Date = date;
            this.Value = value;
        }

        public string TickerSymbol { get; }
        public DateTime Date { get; }
        public decimal Value { get; }
    }
}
