using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Context;

namespace Plethora.Test.Context
{
    [TestClass]
    public class ContextInfo_Tests
    {
        #region Equals

        [TestMethod]
        public void Equals_SameInstance_AreEqual()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            // Action
            bool areEqual = contextInfo.Equals(contextInfo);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Equals_EqualInstances_AreEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 1.0);

            // Action
            bool areEqual = contextInfo1.Equals(contextInfo2);

            // Assert
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Equals_DifferByName_AreNotEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("other", 1, 1.0);

            // Action
            bool areEqual = contextInfo1.Equals(contextInfo2);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void Equals_DifferByRank_AreNotEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 2, 1.0);

            // Action
            bool areEqual = contextInfo1.Equals(contextInfo2);

            // Assert
            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void Equals_DifferByData_AreNotEqual()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 2.0);

            // Action
            bool areEqual = contextInfo1.Equals(contextInfo2);

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

            // Action
            int hashCode1 = contextInfo1.GetHashCode();
            int hashCode2 = contextInfo2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferByName_NotEqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("other", 1, 1.0);

            // Action
            int hashCode1 = contextInfo1.GetHashCode();
            int hashCode2 = contextInfo2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferByRank_NotEqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 2, 1.0);

            // Action
            int hashCode1 = contextInfo1.GetHashCode();
            int hashCode2 = contextInfo2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferByData_NotEqualHashCodes()
        {
            // Arrange
            ContextInfo contextInfo1 = new ContextInfo("name", 1, 1.0);
            ContextInfo contextInfo2 = new ContextInfo("name", 1, 2.0);

            // Action
            int hashCode1 = contextInfo1.GetHashCode();
            int hashCode2 = contextInfo2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        #endregion
    }
}
