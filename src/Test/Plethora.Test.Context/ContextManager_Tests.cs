using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plethora.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Test.Context
{
    [TestClass]
    public class ContextManager_Tests
    {
        #region ContextProvider

        [TestMethod]
        public void Provider_EnterContext_ContextChangedIsRaised()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);

            bool contextChangedRaised = false;
            contextManager.ContextChanged += (sender, e) => contextChangedRaised = true;

            // Action
            contextProviderMock.Raise(m => m.EnterContext += null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(contextChangedRaised);
        }

        [TestMethod]
        public void Provider_LeaveContext_ContextChangedIsRaised()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);

            bool contextChangedRaised = false;
            contextManager.ContextChanged += (sender, e) => contextChangedRaised = true;

            // Action
            contextProviderMock.Raise(m => m.LeaveContext += null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(contextChangedRaised);
        }

        [TestMethod]
        public void Provider_ContextChanged_NotEntered_ContextChangedIsNotRaised()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);

            bool contextChangedRaised = false;
            contextManager.ContextChanged += (sender, e) => contextChangedRaised = true;

            // Action
            contextProviderMock.Raise(m => m.ContextChanged += null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(contextChangedRaised);
        }

        [TestMethod]
        public void Provider_ContextChanged_Entered_ContextChangedIsRaised()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);

            contextProviderMock.Raise(m => m.EnterContext += null, EventArgs.Empty);

            bool contextChangedRaised = false;
            contextManager.ContextChanged += (sender, e) => contextChangedRaised = true;

            // Action
            contextProviderMock.Raise(m => m.ContextChanged += null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(contextChangedRaised);
        }

        #endregion

        #region ContextAugmenter

        [TestMethod]
        public void GetContexts_Augmenter_AdditionalContext()
        {
            // Arrange
            ContextInfo providerContextInfo = new ContextInfo("ProviderContext", 1, 1.0);
            ContextInfo augmenterContextInfo = new ContextInfo("AugmenterContext", 2, 2.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { providerContextInfo });

            var contextAugmenterMock = new Mock<IContextAugmenter>();
            contextAugmenterMock
                .Setup(m => m.ContextName)
                .Returns("ProviderContext");

            contextAugmenterMock
                .Setup(m => m.Augment(It.IsAny<ContextInfo>()))
                .Returns(new[] { augmenterContextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);
            contextManager.RegisterAugmenter(contextAugmenterMock.Object);

            contextProviderMock.Raise(m => m.EnterContext += null, EventArgs.Empty);

            // Action
            IEnumerable<ContextInfo> contexts = contextManager.GetContexts();

            // Assert
            Assert.IsNotNull(contexts);
            Assert.AreEqual(2, contexts.Count());
            Assert.IsTrue(contexts.Contains(providerContextInfo));
            Assert.IsTrue(contexts.Contains(augmenterContextInfo));

            contextProviderMock.Verify(m => m.Contexts, Times.AtLeastOnce);
            contextAugmenterMock.Verify(m => m.Augment(It.IsAny<ContextInfo>()), Times.AtLeastOnce);
        }

        #endregion

        #region GetContexts

        [TestMethod]
        public void GetContexts_NotEnteredProvider_NothingReturned()
        {
            // Arrange
            ContextInfo contextInfo = new ContextInfo("name", 1, 1.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);

            // Action
            IEnumerable<ContextInfo> contexts = contextManager.GetContexts();

            // Assert
            Assert.IsNotNull(contexts);
            Assert.AreEqual(0, contexts.Count());

            contextProviderMock.Verify(m => m.Contexts, Times.Never);
        }

        [TestMethod]
        public void GetContexts_EnteredProvider_ContextsReturned()
        {
            // Arrange
            ContextInfo contextProvider1Info = new ContextInfo("name", 1, 1.0);

            var contextProvider1Mock = new Mock<IContextProvider>();
            contextProvider1Mock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextProvider1Info });

            ContextInfo contextProvider2Info = new ContextInfo("name", 2, 2.0);

            var contextProvider2Mock = new Mock<IContextProvider>();
            contextProvider2Mock
                .Setup(m => m.Contexts)
                .Returns(new[] { contextProvider2Info });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProvider1Mock.Object);
            contextManager.RegisterProvider(contextProvider2Mock.Object);

            contextProvider1Mock.Raise(m => m.EnterContext += null, EventArgs.Empty);

            // Action
            IEnumerable<ContextInfo> contexts = contextManager.GetContexts();

            // Assert
            Assert.IsNotNull(contexts);
            Assert.AreEqual(1, contexts.Count());
            Assert.AreSame(contextProvider1Info, contexts.First());

            contextProvider1Mock.Verify(m => m.Contexts, Times.AtLeastOnce);
            contextProvider2Mock.Verify(m => m.Contexts, Times.Never);
        }


        [TestMethod]
        public void GetContexts_ResultsCached()
        {
            // Arrange
            ContextInfo providerContextInfo = new ContextInfo("ProviderContext", 1, 1.0);
            ContextInfo augmenterContextInfo = new ContextInfo("AugmenterContext", 2, 2.0);

            var contextProviderMock = new Mock<IContextProvider>();
            contextProviderMock
                .Setup(m => m.Contexts)
                .Returns(new[] { providerContextInfo });

            var contextAugmenterMock = new Mock<IContextAugmenter>();
            contextAugmenterMock
                .Setup(m => m.ContextName)
                .Returns("ProviderContext");

            contextAugmenterMock
                .Setup(m => m.Augment(It.IsAny<ContextInfo>()))
                .Returns(new[] { augmenterContextInfo });

            var contextManager = new ContextManager();
            contextManager.RegisterProvider(contextProviderMock.Object);
            contextManager.RegisterAugmenter(contextAugmenterMock.Object);

            contextProviderMock.Raise(m => m.EnterContext += null, EventArgs.Empty);

            // Action
            IEnumerable<ContextInfo> contexts1 = contextManager.GetContexts();

            // Assert
            Assert.IsNotNull(contexts1);
            Assert.AreEqual(2, contexts1.Count());

            contextProviderMock.Verify(m => m.Contexts, Times.Once);
            contextAugmenterMock.Verify(m => m.Augment(It.IsAny<ContextInfo>()), Times.Once);


            contextProviderMock.Invocations.Clear();
            contextAugmenterMock.Invocations.Clear();

            // Action
            IEnumerable<ContextInfo> contexts2 = contextManager.GetContexts();

            // Assert
            Assert.IsNotNull(contexts2);
            Assert.IsTrue(Enumerable.SequenceEqual(contexts1, contexts2));

            contextProviderMock.Verify(m => m.Contexts, Times.Never);
            contextAugmenterMock.Verify(m => m.Augment(It.IsAny<ContextInfo>()), Times.Never);
        }

        #endregion
    }
}
