using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class SortedByKeyListHelper_Test
    {
        [Test]
        public void GetByRange_UniqueList_Inclusive()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 07), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(3, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(2).DateTime);
        }

        [Test]
        public void GetByRange_UniqueList_Exclusive()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 07), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);

            //Test
            Assert.AreEqual(1, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
        }

        [Test]
        public void GetByRange_DuplicateList_Inclusive()
        {
            //Setup
            var list = CreateDuplicateList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 07), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(5, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(4).DateTime);
        }

        [Test]
        public void GetByRange_DuplicateList_Exclusive()
        {
            //Setup
            var list = CreateDuplicateList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 07), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);

            //Test
            Assert.AreEqual(2, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
        }

        [Test]
        public void GetByRange_MinUnavailable()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 05), new DateTime(2000, 01, 07), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(2, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(1).DateTime);
        }

        [Test]
        public void GetByRange_MaxUnavailable()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 05), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(1, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        }

        [Test]
        public void GetByRange_MinBeforeStart()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(1900, 01, 01), new DateTime(2000, 01, 07), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(6, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(4).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(5).DateTime);
        }

        [Test]
        public void GetByRange_MaxPastEnd()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2100, 01, 01), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(4, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(3).DateTime);
        }



        #region Factory Methods

        private SortedByKeyList<DateTime, Price> CreateUniqueList()
        {
            var list = new SortedByKeyList<DateTime, Price>(p => p.DateTime, DuplicatesPolicy.Error, Comparer<DateTime>.Default);
            list.Add(new Price(new DateTime(2000, 01, 01)));
            list.Add(new Price(new DateTime(2000, 01, 02)));
            list.Add(new Price(new DateTime(2000, 01, 03)));
            list.Add(new Price(new DateTime(2000, 01, 04)));
            //Nothing on the Fifth
            list.Add(new Price(new DateTime(2000, 01, 06)));
            list.Add(new Price(new DateTime(2000, 01, 07)));
            list.Add(new Price(new DateTime(2000, 01, 08)));

            return list;
        }

        private SortedByKeyList<DateTime, Price> CreateDuplicateList()
        {
            var list = new SortedByKeyList<DateTime, Price>(p => p.DateTime, DuplicatesPolicy.Allow, Comparer<DateTime>.Default);
            list.Add(new Price(new DateTime(2000, 01, 01)));
            list.Add(new Price(new DateTime(2000, 01, 02)));
            list.Add(new Price(new DateTime(2000, 01, 03)));
            list.Add(new Price(new DateTime(2000, 01, 04)));
            list.Add(new Price(new DateTime(2000, 01, 04)));
            //Nothing on the Fifth
            list.Add(new Price(new DateTime(2000, 01, 06)));
            list.Add(new Price(new DateTime(2000, 01, 06)));
            list.Add(new Price(new DateTime(2000, 01, 07)));
            list.Add(new Price(new DateTime(2000, 01, 08)));

            return list;
        }
        #endregion

        #region Classes
        
        private class Price
        {
            private readonly DateTime dateTime;

            public Price(DateTime dateTime)
            {
                this.dateTime = dateTime;
            }

            public DateTime DateTime
            {
                get { return dateTime; }
            }
        }
        
        #endregion
    }
}
