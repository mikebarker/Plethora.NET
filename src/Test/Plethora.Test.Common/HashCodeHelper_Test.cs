using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
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

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void GetHashCode_Null()
        {
            HashCodeHelper.GetHashCode<object>(null);
        }

        [Test]
        public void GetHashCode_1_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1);
            int hash2 = HashCodeHelper.GetHashCode(item1);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_1_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1);
            int hash2 = HashCodeHelper.GetHashCode(new object());

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_2_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_2_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_3_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_3_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_4_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_4_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_5_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_5_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_6_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_6_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5, item6);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_7_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_7_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5, item6, item7);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_8_Equal()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7, item8);
            int hash2 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7, item8);

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_8_NotEqual()
        {
            //exec
            int hash1 = HashCodeHelper.GetHashCode(item1, item2, item3, item4, item5, item6, item7, item8);
            int hash2 = HashCodeHelper.GetHashCode(new object(), item2, item3, item4, item5, item6, item7, item8);

            //test
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
