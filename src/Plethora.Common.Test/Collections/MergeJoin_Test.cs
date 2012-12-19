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

        [SetUp]
        public void SetUp()
        {
            unorderedListA = new List<Person>();
            unorderedListB = new List<Person>();

            unorderedListA.Add(Person.Bob_Jameson);
            unorderedListA.Add(Person.Amy_Cathson);
            unorderedListA.Add(Person.Jill_Dorrman);

            unorderedListB.Add(Person.Fred_Carlile);
            unorderedListB.Add(Person.Jane_Doe);
            unorderedListB.Add(Person.Katherine_Harold);
            unorderedListB.Add(Person.Bob_Jameson);
            unorderedListB.Add(Person.Harry_Porker);
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
                        expectedKey = Person.Bob_Jameson.ID;
                        expectedMergeType = MergeType.Match;
                        expectedLeftValue = Person.Bob_Jameson;
                        expectedRightValue = Person.Bob_Jameson;
                        break;

                    case 1: //Fred
                        expectedKey = Person.Fred_Carlile.ID;
                        expectedMergeType = MergeType.RightOnly;
                        expectedLeftValue = null;
                        expectedRightValue = Person.Fred_Carlile;
                        break;

                    case 2: //Amy
                        expectedKey = Person.Amy_Cathson.ID;
                        expectedMergeType = MergeType.LeftOnly;
                        expectedLeftValue = Person.Amy_Cathson;
                        expectedRightValue = null;
                        break;

                    case 3: //Jill / Jane
                        expectedKey = Person.Jill_Dorrman.ID;
                        expectedMergeType = MergeType.Different;
                        expectedLeftValue = Person.Jill_Dorrman;
                        expectedRightValue = Person.Jane_Doe;
                        break;

                    case 4: //Katherine
                        expectedKey = Person.Katherine_Harold.ID;
                        expectedMergeType = MergeType.RightOnly;
                        expectedLeftValue = null;
                        expectedRightValue = Person.Katherine_Harold;
                        break;

                    case 5: //Harry
                        expectedKey = Person.Harry_Porker.ID;
                        expectedMergeType = MergeType.RightOnly;
                        expectedLeftValue = null;
                        expectedRightValue = Person.Harry_Porker;
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
