using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Cache.Spacial;
using Plethora.Collections.Sets;
using Plethora.Spacial;
using Plethora.Test.UtilityClasses;
using System;
using System.Linq;

namespace Plethora.Test.Cache.Spacial
{
    [TestClass]
    public class SpacialArgument_Test
    {
        [TestMethod]
        public void IsDataIncluded_IsIncluded()
        {
            // Arrange
            SpacialArgument<Price, string, DateTime> arg = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            // Action
            var result = arg.IsDataIncluded(new Price("MSFT", Dates.Jun07, 12.0m));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDataIncluded_Dimension1Miss_NotIncluded()
        {
            // Arrange
            SpacialArgument<Price, string, DateTime> arg = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            // Action
            var result = arg.IsDataIncluded(new Price("AAPL", Dates.Jun07, 12.0m));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDataIncluded_Dimension2Miss_NotIncluded()
        {
            // Arrange
            SpacialArgument<Price, string, DateTime> arg = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            // Action
            var result = arg.IsDataIncluded(new Price("MSFT", Dates.Dec31, 12.0m));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsOverlapped_ExactMatch()
        {
            // Arrange
            SpacialArgument<Price, string, DateTime> argA = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            SpacialArgument<Price, string, DateTime> argB = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            // Action
            var result = argA.IsOverlapped(argB, out var remainder);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, remainder.Count());
        }

        [TestMethod]
        public void IsOverlapped_ACoversMoreThanB()
        {
            // Arrange
            SpacialArgument<Price, string, DateTime> argA = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            SpacialArgument<Price, string, DateTime> argB = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Dec31)));

            // Action
            var result = argA.IsOverlapped(argB, out var remainder);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, remainder.Count());
        }

        [TestMethod]
        public void IsOverlapped_ACoversLessThanB()
        {
            // Arrange
            SpacialArgument<Price, string, DateTime> argA = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Dec31)));

            SpacialArgument<Price, string, DateTime> argB = new SpacialArgument<Price, string, DateTime>(
                price => Tuple.Create(price.TickerSymbol, price.Date),
                new SpaceRegion<string, DateTime>(
                    new InclusiveSet<string>("MSFT"),
                    new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Jul31)));

            // Action
            var result = argA.IsOverlapped(argB, out var remainder);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, remainder.Count());
            Assert.IsTrue(remainder.First().Region.Dimension1.Contains("MSFT"));
            Assert.IsFalse(remainder.First().Region.Dimension2.Contains(Dates.Jul31));
            Assert.IsTrue(remainder.First().Region.Dimension2.Contains(Dates.Aug01));
            Assert.IsTrue(remainder.First().Region.Dimension2.Contains(Dates.Dec31));
        }
    }
}
