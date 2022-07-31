using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using System;
using System.Linq;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class DropoutStack_Test
    {
        [TestMethod]
        public void InitialState()
        {
            // Arrange

            // Action
            var stack = new DropoutStack<string>(5);

            // Assert
            Assert.AreEqual(0, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
        }

        [TestMethod]
        public void Push_BelowCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");

            // Action

            // Assert
            Assert.AreEqual(3, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "three", "two", "one" }, stack));
        }

        [TestMethod]
        public void Push_ToCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");

            // Action

            // Assert
            Assert.AreEqual(5, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "five", "four", "three", "two", "one" }, stack));
        }

        [TestMethod]
        public void Push_PastCapacity_LeastRecentDropped()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");
            stack.Push("six");

            // Action

            // Assert
            Assert.AreEqual(5, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "six", "five", "four", "three", "two" }, stack));
        }

        [TestMethod]
        public void Pop_BelowCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");

            // Action
            var popped = stack.Pop();

            // Assert
            Assert.AreEqual("three", popped);
            Assert.AreEqual(2, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "two", "one" }, stack));
        }

        [TestMethod]
        public void Pop_AtCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");

            // Action
            var popped = stack.Pop();

            // Assert
            Assert.AreEqual("five", popped);
            Assert.AreEqual(4, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "four", "three", "two", "one" }, stack));
        }

        [TestMethod]
        public void Pop_PastCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");
            stack.Push("six");

            // Action
            var popped = stack.Pop();

            // Assert
            Assert.AreEqual("six", popped);
            Assert.AreEqual(4, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { "five", "four", "three", "two" }, stack));
        }

        [TestMethod]
        public void Pop_ToEmpty()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");
            stack.Push("six");

            _ = stack.Pop(); // six
            _ = stack.Pop(); // five
            _ = stack.Pop(); // four
            _ = stack.Pop(); // three
            _ = stack.Pop(); // two


            // Action

            // Assert
            Assert.AreEqual(0, stack.Count);
            Assert.AreEqual(5, stack.Capacity);
            Assert.IsTrue(Enumerable.SequenceEqual(Array.Empty<string>(), stack));
        }

        [TestMethod]
        public void Pop_PastEmpty()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");
            stack.Push("six");

            _ = stack.Pop(); // six
            _ = stack.Pop(); // five
            _ = stack.Pop(); // four
            _ = stack.Pop(); // three
            _ = stack.Pop(); // two


            // Action
            try
            {
                _ = stack.Pop();
                
                // Assert
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void Peek_BelowCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");

            // Action
            var peeked = stack.Peek();

            // Assert
            Assert.AreEqual("three", peeked);
            Assert.AreEqual(3, stack.Count);
        }

        [TestMethod]
        public void Peek_AtCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");

            // Action
            var peeked = stack.Peek();

            // Assert
            Assert.AreEqual("five", peeked);
            Assert.AreEqual(5, stack.Count);
        }

        [TestMethod]
        public void Peek_PastCapacity()
        {
            // Arrange
            var stack = new DropoutStack<string>(5);
            stack.Push("one");
            stack.Push("two");
            stack.Push("three");
            stack.Push("four");
            stack.Push("five");
            stack.Push("six");

            // Action
            var peeked = stack.Peek();

            // Assert
            Assert.AreEqual("six", peeked);
            Assert.AreEqual(5, stack.Count);
        }
    }
}
