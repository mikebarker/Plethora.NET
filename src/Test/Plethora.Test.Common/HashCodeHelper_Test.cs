using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
    public class HashCodeHelper_Test
    {
        private readonly object item1 = new object();
        private readonly int item2 = 2;
        private readonly string item3 = "item 3";
        private readonly float item4 = 4.0f;
        private readonly double item5 = 5.0;
        private readonly decimal item6 = 6m;
        private readonly long item7 = 7L;
        private readonly int? item8 = null;

        [TestMethod]
        public void GetHashCode_Null()
        {
            HashCodeHelper.GetHashCode<object>(null);
        }

        [TestMethod]
        public void GetHashCode_1_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1);
            int hash2 = HashCodeHelper.GetHashCode(item1);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_1_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1);
            int hash2 = HashCodeHelper.GetHashCode(new object());

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_2_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_2_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_3_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_3_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_4_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_4_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_5_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_5_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_6_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_6_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5, item6);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_7_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_7_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5, item6, item7);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_8_Equal()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7, item8);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7, item8);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_8_NotEqual()
        {
            // Action
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7, item8);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5, item6, item7, item8);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
