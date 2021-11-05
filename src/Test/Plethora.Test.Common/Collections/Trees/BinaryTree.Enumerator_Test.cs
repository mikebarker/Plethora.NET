using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestClass]
    public class BinaryTreeEnumerator_Test
    {
        private IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator;

        public BinaryTreeEnumerator_Test()
        {
            BinaryTree<string, int> tree = new BinaryTree<string, int>
                       {
                           {"Harry", 7},
                           {"Mark", 12},
                           {"Jeff", 14}
                       };

            this.enumerator = tree.GetPairEnumerator();
        }

        [TestMethod]
        public void All()
        {
            // Arrange

            // Action and Test
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

        [TestMethod]
        public void LimitMin()
        {
            // Arrange
            this.enumerator.Min = "I"; // excludes "Harry"

            // Action and Test
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

        [TestMethod]
        public void LimitMax()
        {
            // Arrange
            this.enumerator.Max = "L"; // excludes "Mark"

            // Action and Test
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

        [TestMethod]
        public void LimitMinAndMax()
        {
            // Arrange
            this.enumerator.Min = "I"; // excludes "Harry"
            this.enumerator.Max = "L"; // excludes "Mark"

            // Action and Test
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
