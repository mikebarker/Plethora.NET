using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Transformations;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Plethora.Test.Collections.Transformations
{
    [TestClass]
    public class FilteredTransformedList_Test
    {
        private static readonly Func<int, bool> IncludeEvenPredicate = (x) => (x % 2) == 0;

        [TestMethod]
        public void Initialise_FilterApplied()
        {
            // Arrange
            ObservableCollection<int> source = new([1, 2, 3, 4, 5, 6, 7, 8, 9]);


            // Action
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Assert
            Assert.IsTrue(filteredList.SequenceEqual([2, 4, 6, 8]));
            Assert.AreEqual(4, filteredList.Count);
        }

        [TestMethod]
        public void AddToSource_ItemIncluded()
        {
            // Arrange
            ObservableCollection<int> source = new();
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Add(2);

            // Assert
            Assert.IsTrue(source.SequenceEqual([ 2 ]));
            Assert.IsTrue(filteredList.SequenceEqual([ 2 ]));

            Assert.AreEqual(1, filteredList.Count);
        }

        [TestMethod]
        public void AddToSource_ItemExcluded()
        {
            // Arrange
            ObservableCollection<int> source = new();
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Add(3);

            // Assert
            Assert.IsTrue(source.SequenceEqual([ 3 ]));
            Assert.IsTrue(filteredList.SequenceEqual(Array.Empty<int>()));

            Assert.AreEqual(0, filteredList.Count);
        }

        [TestMethod]
        public void Add_ItemIncluded()
        {
            // Arrange
            ObservableCollection<int> source = new();
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            filteredList.Add(2);

            // Assert
            Assert.IsTrue(source.SequenceEqual([ 2 ]));
            Assert.IsTrue(filteredList.SequenceEqual([ 2 ]));

            Assert.AreEqual(1, filteredList.Count);
        }

        [TestMethod]
        public void Add_ItemExcluded()
        {
            // Arrange
            ObservableCollection<int> source = new();
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            filteredList.Add(3);

            // Assert
            Assert.IsTrue(source.SequenceEqual([ 3 ]));
            Assert.IsTrue(filteredList.SequenceEqual(Array.Empty<int>()));

            Assert.AreEqual(0, filteredList.Count);
        }

        [TestMethod]
        public void AddToSource_InsertOrderMaintained()
        {
            // Arrange
            ObservableCollection<int> source = new([1, 2, 3, 7, 8, 9]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Insert(3, 6);
            source.Insert(3, 5);
            source.Insert(3, 4);

            // Assert
            Assert.IsTrue(filteredList.SequenceEqual([2, 4, 6, 8]));
        }

        [TestMethod]
        public void ClearSource()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Clear();

            // Assert
            Assert.IsTrue(source.SequenceEqual(Array.Empty<int>()));
            Assert.IsTrue(filteredList.SequenceEqual(Array.Empty<int>()));

            Assert.AreEqual(0, filteredList.Count);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            ObservableCollection<int> source = new([1, 2, 3, 4, 5, 6, 7, 8, 9]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            filteredList.Clear();

            // Assert
            Assert.IsTrue(source.SequenceEqual(Array.Empty<int>()));
            Assert.IsTrue(filteredList.SequenceEqual(Array.Empty<int>()));

            Assert.AreEqual(0, filteredList.Count);
        }

        [TestMethod]
        public void Contains_ItemIncluded()
        {
            // Arrange
            ObservableCollection<int> source = new([1, 2, 3, 4, 5, 6, 7, 8, 9]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            var result = filteredList.Contains(2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_ItemExcluded()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            var result = filteredList.Contains(3);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveFromSource_ItemIncluded()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Remove(2);

            // Assert
            Assert.IsTrue(source.SequenceEqual([1, 3, 4, 5, 6, 7, 8, 9]));
            Assert.IsTrue(filteredList.SequenceEqual([4, 6, 8]));

            Assert.AreEqual(3, filteredList.Count);
        }

        [TestMethod]
        public void RemoveFromSource_ItemExcluded()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Remove(3);

            // Assert
            Assert.IsTrue(source.SequenceEqual([1, 2, 4, 5, 6, 7, 8, 9]));
            Assert.IsTrue(filteredList.SequenceEqual([2, 4, 6, 8]));

            Assert.AreEqual(4, filteredList.Count);
        }

        [TestMethod]
        public void Remove_ItemIncluded()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            filteredList.Remove(2);

            // Assert
            Assert.IsTrue(source.SequenceEqual([1, 3, 4, 5, 6, 7, 8, 9]));
            Assert.IsTrue(filteredList.SequenceEqual([4, 6, 8]));

            Assert.AreEqual(3, filteredList.Count);
        }

        [TestMethod]
        public void Remove_ItemExcluded()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            filteredList.Remove(3);

            // Assert
            Assert.IsTrue(source.SequenceEqual([1, 2, 4, 5, 6, 7, 8, 9]));
            Assert.IsTrue(filteredList.SequenceEqual([2, 4, 6, 8]));

            Assert.AreEqual(4, filteredList.Count);
        }

        [TestMethod]
        public void RemoveFromSource_OrderMaintained()
        {
            // Arrange
            ObservableCollection<int> source = new([ 1, 2, 3, 4, 5, 6, 7, 8, 9 ]);
            FilteredTransformedList<int> filteredList = new(source, IncludeEvenPredicate);

            // Action
            source.Remove(5);
            source.Remove(6);
            source.Remove(4);

            // Assert
            Assert.IsTrue(source.SequenceEqual([1, 2, 3, 7, 8, 9]));
            Assert.IsTrue(filteredList.SequenceEqual([2, 8]));
        }
    }
}
