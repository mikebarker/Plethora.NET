using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class MruDictionary_Test
    {
        [Test]
        public void AddUptoMaxEntries()
        {
            //setup
            Random random = new Random(1778);  // constant makes tests repeatable, but has no significance.
            var mruDictionary = new MruDictionary<string, string>(maxEntries: 100);

            Populate(mruDictionary, random, 99);

            //exec
            string str = GeneratRandomString(random, 8);
            mruDictionary.Add(str, str);

            //test
            Assert.AreEqual(100, mruDictionary.Count);
            Assert.IsTrue(mruDictionary.ContainsKey(str));
        }

        [Test]
        public void AddOverMaxEntries()
        {
            //setup
            Random random = new Random(1778);  // constant makes tests repeatable, but has no significance.
            var mruDictionary = new MruDictionary<string, string>(maxEntries: 100, watermark: 70);

            Populate(mruDictionary, random, 100);

            //exec
            string str = GeneratRandomString(random, 8);
            mruDictionary.Add(str, str);

            //test
            Assert.AreEqual(70, mruDictionary.Count);
            Assert.IsTrue(mruDictionary.ContainsKey(str));
        }

        [Test]
        public void DropLeastUsed()
        {
            //setup
            //-----
            Random random = new Random(1778);  // constant makes tests repeatable, but has no significance.
            var mruDictionary = new MruDictionary<string, string>(maxEntries: 100, watermark: 70);

            Populate(mruDictionary, random, 100);

            //exec
            //----
            ICollection<string> keys = mruDictionary.Keys;  // Keys collection doesn not impact access count

            int unaccessedKeyIndex = random.Next(0, keys.Count);
            string unaccessedKey = keys.ElementAt(unaccessedKeyIndex);

            //Access all but one item
            foreach (string key in keys.Where(k => k != unaccessedKey))
            {
                string value = mruDictionary[key];  // Item access increases access count
            }

            //Add a new item, over the MaxEnteries limit
            string str = GeneratRandomString(random, 8);
            mruDictionary.Add(str, str);

            //test
            //----
            //Ensure the single item not access was one of the ones removed.
            Assert.IsFalse(mruDictionary.ContainsKey(unaccessedKey));
        }

        #region

        private static readonly char[] AlphaNumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        private void Populate(IDictionary<string, string> dictionary, Random random, int numberOfElements)
        {
            for (int i = 0; i < numberOfElements; i++)
            {
                string str = GeneratRandomString(random, 8);
                dictionary.Add(str, str);
            }
        }

        private string GeneratRandomString(Random random, int length)
        {
            char[] strArray = new char[length];

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, AlphaNumerics.Length);

                strArray[i] = AlphaNumerics[index];
            }
            return new string(strArray);
        }

        #endregion
    }
}
