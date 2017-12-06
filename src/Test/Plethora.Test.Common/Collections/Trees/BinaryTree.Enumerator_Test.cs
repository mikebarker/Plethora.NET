using System.Collections.Generic;
using NUnit.Framework;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestFixture]
    public class BinaryTreeEnumerator_Test
    {
        private IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator;

        [SetUp]
        public void SetUp()
        {
            BinaryTree<string, int> tree = new BinaryTree<string, int>
                       {
                           {"Harry", 7},
                           {"Mark", 12},
                           {"Jeff", 14}
                       };

            this.enumerator = tree.GetPairEnumerator();
        }

        [Test]
        public void All()
        {
            //Setup

            //Execute and Test
            int count = 0;
            while(this.enumerator.MoveNext())
            {
                int currentValue = this.enumerator.Current.Value;

                if (count == 0)
                    Assert.AreEqual(currentValue, 7);  //Harry
                if (count == 1)
                    Assert.AreEqual(currentValue, 14); //Jeff
                if (count == 2)
                    Assert.AreEqual(currentValue, 12); //Mark

                count++;
            }

            Assert.AreEqual(count, 3);
        }

        [Test]
        public void LimitMin()
        {
            //Setup
            this.enumerator.Min = "I"; // excludes "Harry"

            //Execute and Test
            int count = 0;
            while(this.enumerator.MoveNext())
            {
                int currentValue = this.enumerator.Current.Value;

                if (count == 0)
                    Assert.AreEqual(currentValue, 14); //Jeff
                if (count == 1)
                    Assert.AreEqual(currentValue, 12); //Mark

                count++;
            }

            Assert.AreEqual(count, 2);
        }

        [Test]
        public void LimitMax()
        {
            //Setup
            this.enumerator.Max = "L"; // excludes "Mark"

            //Execute and Test
            int count = 0;
            while (this.enumerator.MoveNext())
            {
                int currentValue = this.enumerator.Current.Value;

                if (count == 0)
                    Assert.AreEqual(currentValue, 7);  //Harry
                if (count == 1)
                    Assert.AreEqual(currentValue, 14); //Jeff

                count++;
            }

            Assert.AreEqual(count, 2);
        }

        [Test]
        public void LimitMinAndMax()
        {
            //Setup
            this.enumerator.Min = "I"; // excludes "Harry"
            this.enumerator.Max = "L"; // excludes "Mark"

            //Execute and Test
            int count = 0;
            while (this.enumerator.MoveNext())
            {
                int currentValue = this.enumerator.Current.Value;

                if (count == 0)
                    Assert.AreEqual(currentValue, 14); //Jeff

                count++;
            }

            Assert.AreEqual(count, 1);
        }

    }
}
