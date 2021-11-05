using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Context;
using System;
using System.Collections.Generic;

namespace Plethora.Test.Context
{
    [TestClass]
    public class ContextInfoIgnoreRankComparer_Tests
    {
        #region Equals

        [TestMethod]
        public void Equals_SameInstance_AreEqual()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            bool areEqual = comparer.Equals(contextInfo, contextInfo);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Equals_EqualInstances_AreEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            bool areEqual = comparer.Equals(contextInfo1, contextInfo2);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Equals_DifferByRank_AreEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 2, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            bool areEqual = comparer.Equals(contextInfo1, contextInfo2);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Equals_DifferByName_AreNotEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("other", 1, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            bool areEqual = comparer.Equals(contextInfo1, contextInfo2);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void Equals_DifferByData_AreNotEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 2.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            bool areEqual = comparer.Equals(contextInfo1, contextInfo2);

            // Assert
            Assert.IsFalse(areEqual);
        }

        #endregion

        #region GetHashCode

        [TestMethod]
        public void GetHashCode_EqualInstances_EqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            int hashCode1 = comparer.GetHashCode(contextInfo1);
            int hashCode2 = comparer.GetHashCode(contextInfo2);

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferByRank_EqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 2, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            int hashCode1 = comparer.GetHashCode(contextInfo1);
            int hashCode2 = comparer.GetHashCode(contextInfo2);

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferByName_NotEqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("other", 1, 1.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            int hashCode1 = comparer.GetHashCode(contextInfo1);
            int hashCode2 = comparer.GetHashCode(contextInfo2);

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferByData_NotEqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 2.0);

            IEqualityComparer<ContextInfo> comparer = CreateContextInfoIgnoreRankComparer();

            // Action
            int hashCode1 = comparer.GetHashCode(contextInfo1);
            int hashCode2 = comparer.GetHashCode(contextInfo2);

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        #endregion

        #region Factory Methods

        private static IEqualityComparer<ContextInfo> CreateContextInfoIgnoreRankComparer()
        {
            var assembly = typeof(ContextInfo).Assembly;
            var type = assembly.GetType("Plethora.Context.ContextInfoIgnoreRankComparer", true);

            var obj = Activator.CreateInstance(type);
            var comparer = (IEqualityComparer<ContextInfo>)obj;
            return comparer;
        }

        #endregion
    }
}
