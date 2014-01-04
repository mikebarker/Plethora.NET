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
        public void GetByKey_EmptyList()
        {
            //Setup
            var list = CreateEmptyList();
            var date = new DateTime(2000, 01, 04);

            //Exec
            IEnumerable<Price> subList = list.GetByKey(date);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByKey_UniqueList()
        {
            //Setup
            var list = CreateUniqueList();
            var date = new DateTime(2000, 01, 04);

            //Exec
            IEnumerable<Price> subList = list.GetByKey(date);


            //Test
            Assert.AreEqual(1, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        }

        [Test]
        public void GetByKey_UniqueList_NotInRange()
        {
            //Setup
            var list = CreateUniqueList();
            var date = new DateTime(2100, 01, 04);

            //Exec
            IEnumerable<Price> subList = list.GetByKey(date);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByKey_DuplicateList_MultipleResults()
        {
            //Setup
            var list = CreateDuplicateList();
            var date = new DateTime(2000, 01, 04);

            //Exec
            IEnumerable<Price> subList = list.GetByKey(date);


            //Test
            Assert.AreEqual(2, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(1).DateTime);
        }


        [Test]
        public void GetByRange_EmptyList_Inclusive()
        {
            //Setup
            var list = CreateEmptyList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 07), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByRange_EmptyList_Exclusive()
        {
            //Setup
            var list = CreateEmptyList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 07), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

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
        public void GetByRange_Inclusive_MinUnavailable()
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
        public void GetByRange_Inclusive_MaxUnavailable()
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
        public void GetByRange_Inclusive_MinBeforeStart()
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
        public void GetByRange_Inclusive_MaxBeforeStart()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(1900, 01, 01), new DateTime(1990, 12, 30), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByRange_Inclusive_MinPastEnd()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2010, 01, 04), new DateTime(2100, 01, 01), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByRange_Inclusive_MaxPastEnd()
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

        [Test]
        public void GetByRange_Inclusive_MaxAtEnd()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 08), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(4, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(3).DateTime);
        }

        [Test]
        public void GetByRange_Inclusive_MinAtStart()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 01), new DateTime(2000, 01, 08), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(7, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(4).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(5).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(6).DateTime);
        }


        [Test]
        public void GetByRange_Exclusive_MinUnavailable()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 05), new DateTime(2000, 01, 07), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(1, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
        }

        [Test]
        public void GetByRange_Exclusive_MaxUnavailable()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 05), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByRange_Exclusive_MinBeforeStart()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(1900, 01, 01), new DateTime(2000, 01, 07), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(5, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(4).DateTime);
        }

        [Test]
        public void GetByRange_Exclusive_MaxBeforeStart()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(1900, 01, 01), new DateTime(1990, 12, 30), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByRange_Exclusive_MinPastEnd()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2010, 01, 04), new DateTime(2100, 01, 01), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(0, subList.Count());
        }

        [Test]
        public void GetByRange_Exclusive_MaxPastEnd()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2100, 01, 01), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(3, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(2).DateTime);
        }

        [Test]
        public void GetByRange_Exclusive_MaxAtEnd()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 04), new DateTime(2000, 01, 08), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(2, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(1).DateTime);
        }

        [Test]
        public void GetByRange_Exclusive_MinAtStart()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 01), new DateTime(2000, 01, 08), false, false);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(5, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(4).DateTime);
        }


        
        [Test]
        public void GetByRange_FirstElement()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 01), new DateTime(2000, 01, 01), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(1, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
        }

        [Test]
        public void GetByRange_LastElement()
        {
            //Setup
            var list = CreateUniqueList();
            var range = new Range<DateTime>(new DateTime(2000, 01, 08), new DateTime(2000, 01, 08), true, true);

            //Exec
            IEnumerable<Price> subList = list.GetByRange(range);


            //Test
            Assert.AreEqual(1, subList.Count());

            Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(0).DateTime);
        }


        #region Factory Methods

        private SortedByKeyList<DateTime, Price> CreateEmptyList()
        {
            var list = new SortedByKeyList<DateTime, Price>(p => p.DateTime, DuplicatesPolicy.Error, Comparer<DateTime>.Default);

            return list;
        }

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
