using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
    public class ActionOnDispose_Test
    {
        [TestMethod]
        public void Dispose_NotCalled_ActionNotCalled()
        {
            // Arrange
            bool isCalled = false;

            // Action
            var action = new ActionOnDispose(() => { isCalled = true; });

            // Assert
            Assert.IsFalse(isCalled);
        }

        [TestMethod]
        public void Dispose_ActionCalled_Explicit()
        {
            // Arrange
            bool isCalled = false;
            var action = new ActionOnDispose(() => { isCalled = true; });

            // Action
            action.Dispose();

            // Assert
            Assert.IsTrue(isCalled);
        }

        [TestMethod]
        public void Dispose_ActionCalled_UsingPattern()
        {
            // Arrange
            bool isCalled = false;

            // Action
            using (new ActionOnDispose(() => { isCalled = true; }))
            { 
            }

            // Assert
            Assert.IsTrue(isCalled);
        }

        [TestMethod]
        public void Dispose_Nested()
        {
            // Arrange
            bool isCalled1 = false;
            bool isCalled2 = false;

            // Action
            using (new ActionOnDispose(() => { isCalled1 = true; }))
            {
                using (new ActionOnDispose(() => { isCalled2 = true; }))
                {
                    Assert.IsFalse(isCalled1);
                    Assert.IsFalse(isCalled2);
                }

                Assert.IsFalse(isCalled1);
                Assert.IsTrue(isCalled2);
            }

            Assert.IsTrue(isCalled1);
            Assert.IsTrue(isCalled2);
        }
    }
}
