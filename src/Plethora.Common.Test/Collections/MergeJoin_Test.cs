using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class MergeJoin_Test
    {
        private List<Person> unorderedListA;
        private List<Person> unorderedListB;
        private readonly Person Bob_Jameson = new Person(0, "Jameson", "Bob", new DateTime(1964, 03, 14));
        private readonly Person Fred_Carlile = new Person(1, "Carlile", "Fred", new DateTime(1975, 11, 07));
        private readonly Person Amy_Cathson = new Person(2, "Cathson", "Amy", new DateTime(1984, 02, 21));
        private readonly Person Jill_Dorrman = new Person(3, "Dorrman", "Jill", new DateTime(1978, 05, 08));
        private readonly Person Jane_Doe = new Person(3, "Doe", "Jane", new DateTime(1976, 03, 15));
        private readonly Person Katherine_Harold = new Person(4, "Harold", "Katherine", new DateTime(1984, 02, 21));
        private readonly Person Harry_Porker = new Person(5, "Porker", "Harry", new DateTime(1978, 05, 08));

        [SetUp]
        public void SetUp()
        {
            unorderedListA = new List<Person>();
            unorderedListB = new List<Person>();

            unorderedListA.Add(Bob_Jameson);
            unorderedListA.Add(Amy_Cathson);
            unorderedListA.Add(Jill_Dorrman);

            unorderedListB.Add(Fred_Carlile);
            unorderedListB.Add(Jane_Doe);
            unorderedListB.Add(Katherine_Harold);
            unorderedListB.Add(Bob_Jameson);
            unorderedListB.Add(Harry_Porker);
        }

        [Test]
        public void FindMergeSet()
        {
            //exec
            List<MergeItem<long, Person>> mergeSet = MergeJoin.FindMergeSet(
                unorderedListA,
                unorderedListB,
                person => person.ID);

            //test
            Assert.IsNotNull(mergeSet);
            TestMergeSet(mergeSet);
        }

        [Test]
        public void FindMergeSetPreOrdered()
        {
            var orderedA = unorderedListA.OrderBy(person => person.ID);
            var orderedB = unorderedListB.OrderBy(person => person.ID);

            //exec
            List<MergeItem<long, Person>> mergeSet = MergeJoin.FindMergeSetPreOrdered(
                orderedA,
                orderedB,
                person => person.ID);

            //test
            Assert.IsNotNull(mergeSet);
            TestMergeSet(mergeSet);
        }

        [Test]
        public void Merge()
        {
            //setup
            int matchCount = 0;
            int differentCount = 0;
            int leftOnlyCount = 0;
            int rightOnlyCount = 0;

            Action<long, Person> onMatch = delegate { matchCount++; };
            Action<long, Person, Person> onDifferent = delegate { differentCount++; };
            Action<long, Person> onLeftOnly = delegate { leftOnlyCount++; };
            Action<long, Person> onRightOnly = delegate { rightOnlyCount++; };

            //exec
            MergeJoin.Merge(
                unorderedListA,
                unorderedListB,
                person => person.ID,
                
                onMatch,
                onDifferent,
                onLeftOnly,
                onRightOnly);

            //test
            Assert.AreEqual(1, matchCount);
            Assert.AreEqual(1, differentCount);
            Assert.AreEqual(1, leftOnlyCount);
            Assert.AreEqual(3, rightOnlyCount);
        }

        [Test]
        public void MergePreOrdered()
        {
            //setup
            int matchCount = 0;
            int differentCount = 0;
            int leftOnlyCount = 0;
            int rightOnlyCount = 0;

            Action<long, Person> onMatch = delegate { matchCount++; };
            Action<long, Person, Person> onDifferent = delegate { differentCount++; };
            Action<long, Person> onLeftOnly = delegate { leftOnlyCount++; };
            Action<long, Person> onRightOnly = delegate { rightOnlyCount++; };

            var orderedA = unorderedListA.OrderBy(person => person.ID);
            var orderedB = unorderedListB.OrderBy(person => person.ID);

            //exec
            MergeJoin.MergePreOrdered(
                orderedA,
                orderedB,
                person => person.ID,
                
                onMatch,
                onDifferent,
                onLeftOnly,
                onRightOnly);

            //test
            Assert.AreEqual(1, matchCount);
            Assert.AreEqual(1, differentCount);
            Assert.AreEqual(1, leftOnlyCount);
            Assert.AreEqual(3, rightOnlyCount);
        }

        #region Private Methods

        private void TestMergeSet(List<MergeItem<long, Person>> mergeSet)
        {
            for (int i = 0; i < mergeSet.Count; i++)
            {
                var mergeItem = mergeSet[i];

                long expectedKey = -1;
                MergeType expectedMergeType = (MergeType)(-1);
                Person expectedLeftValue = null;
                Person expectedRightValue = null;

                switch (i)
                {
                    case 0: //Bob
                        expectedKey = Bob_Jameson.ID;
                        expectedMergeType = MergeType.Match;
                        expectedLeftValue = Bob_Jameson;
                        expectedRightValue = Bob_Jameson;
                        break;

                    case 1: //Fred
                        expectedKey = Fred_Carlile.ID;
                        expectedMergeType = MergeType.RightOnly;
                        expectedLeftValue = null;
                        expectedRightValue = Fred_Carlile;
                        break;

                    case 2: //Amy
                        expectedKey = Amy_Cathson.ID;
                        expectedMergeType = MergeType.LeftOnly;
                        expectedLeftValue = Amy_Cathson;
                        expectedRightValue = null;
                        break;

                    case 3: //Jill / Jane
                        expectedKey = Jill_Dorrman.ID;
                        expectedMergeType = MergeType.Different;
                        expectedLeftValue = Jill_Dorrman;
                        expectedRightValue = Jane_Doe;
                        break;

                    case 4: //Katherine
                        expectedKey = Katherine_Harold.ID;
                        expectedMergeType = MergeType.RightOnly;
                        expectedLeftValue = null;
                        expectedRightValue = Katherine_Harold;
                        break;

                    case 5: //Harry
                        expectedKey = Harry_Porker.ID;
                        expectedMergeType = MergeType.RightOnly;
                        expectedLeftValue = null;
                        expectedRightValue = Harry_Porker;
                        break;

                    default:
                        Assert.Fail();
                        return;
                }

                Assert.AreEqual(expectedKey, mergeItem.Key);
                Assert.AreEqual(expectedMergeType, mergeItem.MergeType);
                Assert.AreEqual(expectedLeftValue, mergeItem.LeftValue);
                Assert.AreEqual(expectedRightValue, mergeItem.RightValue);
            }
        }
        #endregion
    }
}
